using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Idea.Repository;
using Idea.Sample.Internals.DbContext;
using Idea.Sample.Internals.Models;
using Idea.SmartQuery.EntityFrameworkCore;
using Idea.UnitOfWork;

using Microsoft.AspNetCore.Mvc;

using TagEntity = Idea.Sample.Internals.Entities.Tag;

namespace Idea.Sample.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWorkFactory _uowFactory;

        private readonly IRepository<TagEntity, Guid> _tagRepository;

        public TagsController(
            IMapper mapper,
            IUnitOfWorkFactory uowFactory, 
            IRepository<TagEntity, Guid> tagRepository)
        {
            _mapper = mapper;
            _uowFactory = uowFactory;
            _tagRepository = tagRepository;
        }
        
        [HttpGet]
        public async Task<IEnumerable<TagRead>> GetAsync()
        {
            using (var uow = _uowFactory.Create())
            {
                var query = new CommonQuery<SampleDbContext, TagEntity, Guid>(i => i.PostTags);
                var data = await query.ExecuteAsync(uow);
                return _mapper.Map<IEnumerable<TagRead>>(data);
            }
        }

        [HttpGet("{id}", Name = "TagGetAsync")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            using (_uowFactory.Create())
            {
                var data = await _tagRepository.FindAsync(id);
                if (data == null)
                {
                    return new NotFoundResult();
                }

                return new JsonResult(_mapper.Map<TagRead>(data));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Tag data)
        {
            var record = _mapper.Map<TagEntity>(data);

            using (var uow = _uowFactory.Create())
            {
                await _tagRepository.CreateAsync(record);
                await uow.CommitAsync();

                return CreatedAtRoute("TagGetAsync", new {record.Id}, new { record.Id });
            }
        }
        
        [HttpPut("{id}")]
        public async Task Put(Guid id, [FromBody] Tag data)
        {
            using (var uow = _uowFactory.Create())
            {
                var tags = await _tagRepository.FindAsync(id);

                _mapper.Map(data, tags);

                await _tagRepository.UpdateAsync(tags);
                await uow.CommitAsync();
            }
        }
        
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            using (var uow = _uowFactory.Create())
            {
                var entity = await _tagRepository.FindAsync(id);
                await _tagRepository.DeleteAsync(entity);

                await uow.CommitAsync();
            }
        }
    }
}
