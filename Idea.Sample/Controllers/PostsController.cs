using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Idea.Repository;
using Idea.Sample.Internals.DbContext;
using Idea.Sample.Internals.Entities;
using Idea.Sample.Internals.Models;
using Idea.Sample.Internals.Queries;
using Idea.SmartQuery;
using Idea.SmartQuery.EntityFrameworkCore;
using Idea.SmartQuery.Interfaces;
using Idea.SmartQuery.QueryData;
using Idea.UnitOfWork;

using Microsoft.AspNetCore.Mvc;

using Post = Idea.Sample.Internals.Models.Post;
using PostEntity = Idea.Sample.Internals.Entities.Post;
using TagEntity = Idea.Sample.Internals.Entities.Tag;
using PostTagEntity = Idea.Sample.Internals.Entities.PostTag;

namespace Idea.Sample.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWorkFactory _uowFactory;

        private readonly IQueryFactory _queryFactory;

        private readonly IRepository<PostEntity, Guid> _postRepository;

        private readonly IRepository<PostTagEntity, Guid> _postTagRepository;

        public PostsController(
            IMapper mapper,
            IQueryFactory queryFactory,
            IUnitOfWorkFactory uowFactory, 
            IRepository<PostEntity, Guid> postRepository,
            IRepository<PostTagEntity, Guid> postTagRepository)
        {
            _mapper = mapper;
            _uowFactory = uowFactory;
            _queryFactory = queryFactory;
            _postRepository = postRepository;
            _postTagRepository = postTagRepository;
        }
        
        [HttpGet]
        public async Task<IEnumerable<PostRead>> GetAsync()
        {
            using (var uow = _uowFactory.Create())
            {
                var query = new CommonQuery<SampleDbContext, PostEntity, Guid>(i => i.PostTags);
                var data = await query.ExecuteAsync(uow);
                return _mapper.Map<IEnumerable<PostRead>>(data);
            }
        }

        [HttpGet("{id}", Name = "PostGetAsync")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            using (var uow = _uowFactory.Create())
            {
                var query = new CommonQuery<SampleDbContext, PostEntity, Guid>(w => w.Id == id, i => i.PostTags);
                var data = await query.ExecuteAsync(uow);
                if (!data.Any())
                {
                    return new NotFoundResult();
                }

                var entity = data.First();
                return new JsonResult(_mapper.Map<PostRead>(entity));
            }
        }

        [HttpGet("{id}/all")]
        public async Task<IActionResult> GetAllDetailsAsync(Guid id)
        {
            using (var uow = _uowFactory.Create())
            {
                var query = _queryFactory
                    .CreateQuery<PostById, PostEntity, Guid>(
                        new QueryReader<IQueryData>(() => new ById<Guid>(id)));
                var data = await query.ExecuteAsync(uow);
                if (!data.Any())
                {
                    return new NotFoundResult();
                }

                var entity = data.First();
                return new JsonResult(_mapper.Map<PostRead>(entity));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Post data)
        {
            var record = _mapper.Map<PostEntity>(data);

            using (var uow = _uowFactory.Create())
            {
                await _postRepository.CreateAsync(record);
                await uow.CommitAsync();

                return CreatedAtRoute("PostGetAsync", new { record.Id }, new { record.Id });
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Post model)
        {
            using (var uow = _uowFactory.Create())
            {
                var query = new CommonQuery<SampleDbContext, TagEntity, Guid>(w => model.Tags.Contains(w.Id));
                var tags = await query.ExecuteAsync(uow);
                if (!tags.Select(s => s.Id).All(model.Tags.Contains))
                {
                    return new BadRequestResult();
                }

                var post = _queryFactory.CreateQuery<PostById, PostEntity, Guid>(
                    new QueryReader<ById<Guid>>(() => new ById<Guid>(id)));

                var posts = await post.ExecuteAsync(uow);
                if (!posts.Any())
                {
                    return new NotFoundResult();
                }

                var entity = posts.First();
                var connection = tags.Select(s => new PostTag { PostId = id, TagId = s.Id }).ToList();

                _mapper.Map(model, entity);
                entity.PostTags.ForEach(async a => await _postTagRepository.DeleteAsync(a));
                entity.PostTags = connection;

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
