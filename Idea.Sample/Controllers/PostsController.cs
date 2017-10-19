using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Idea.Repository;
using Idea.Repository.Extensions;
using Idea.Sample.Internals.Models;
using Idea.Sample.Internals.Queries;
using Idea.SmartQuery;
using Idea.SmartQuery.Interfaces;
using Idea.SmartQuery.QueryData;
using Idea.UnitOfWork;

using Microsoft.AspNetCore.Mvc;

using PostEntity = Idea.Sample.Internals.Entities.Post;
using TagEntity = Idea.Sample.Internals.Entities.Tag;

namespace Idea.Sample.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWorkFactory _uowFactory;

        private readonly IQueryFactory _queryFactory;

        private readonly IRepository<PostEntity, Guid> _postRepository;

        private readonly IRepository<TagEntity, Guid> _tagRepository;

        public PostsController(
            IMapper mapper,
            IQueryFactory queryFactory,
            IUnitOfWorkFactory uowFactory, 
            IRepository<PostEntity, Guid> postRepository,
            IRepository<TagEntity, Guid> tagRepository)
        {
            _mapper = mapper;
            _uowFactory = uowFactory;
            _queryFactory = queryFactory;
            _postRepository = postRepository;
            _tagRepository = tagRepository;
        }
        
        [HttpGet]
        public async Task<IEnumerable<PostRead>> GetAsync()
        {
            using (_uowFactory.Create())
            {
                var data = await _postRepository.GetAllAsync(i => i.Tags);
                return _mapper.Map<IEnumerable<PostRead>>(data);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            using (_uowFactory.Create())
            {
                var data = await _postRepository.GetAsync(w => w.Id == id, i => i.Tags);
                if (data.Any())
                {
                    var entity = data.First();
                    return new JsonResult(_mapper.Map<PostRead>(entity));
                }

                return new NotFoundResult();
            }
        }

        [HttpGet("{id}/all")]
        public Task<IActionResult> GetAllDetailsAsync(Guid id)
        {
            using (var uow = _uowFactory.Create())
            {
                var query = _queryFactory
                    .CreateQuery<PostById, PostEntity, Guid>(
                        new QueryReader<IQueryData>(() => new GetById<Guid>(id)));
                var data = query.Execute(uow);
                if (!data.Any())
                {
                    return Task.FromResult<IActionResult>(new NotFoundResult());
                }

                var entity = data.First();
                return Task.FromResult<IActionResult>(new JsonResult(_mapper.Map<PostRead>(entity)));
            }
        }
        
        [HttpPost]
        public async Task Post([FromBody] Post data)
        {
            var record = _mapper.Map<PostEntity>(data);

            using (var uow = _uowFactory.Create())
            {
                await _postRepository.CreateAsync(record);
                await uow.CommitAsync();
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Post data)
        {
            using (var uow = _uowFactory.Create())
            {
                var tags = await _tagRepository.GetAsync(w => data.Tags.Contains(w.Id));
                if (!tags.Select(s => s.Id).All(data.Tags.Contains))
                {
                    return new BadRequestResult();
                }

                var entity = await _postRepository.FindAsync(id);
                _mapper.Map(data, entity);

                await _postRepository.UpdateAsync(entity);
                await uow.CommitAsync();

                return new OkResult();
            }
        }
        
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            using (var uow = _uowFactory.Create())
            {
                var entity = await _postRepository.FindAsync(id);
                await _postRepository.DeleteAsync(entity);

                await uow.CommitAsync();
            }
        }
    }
}
