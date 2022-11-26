using DomainLayer.Entities;
using InfrastructureLayer.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Api.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private readonly DatabaseContext _dbContext;

        public QueryHandler(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
        {
            var post = await _dbContext.Posts.Where(x => x.Id == query.Id).Include(x => x.Comments).AsNoTracking().FirstOrDefaultAsync();
            return new List<PostEntity> { post };
        }

        public async Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query)
        {
            return await _dbContext.Posts.Include(x => x.Comments).AsNoTracking().ToListAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query)
        {
            return await _dbContext.Posts.Include(x => x.Comments).Where(x => x.Likes > query.NumberOfLikes).AsNoTracking().ToListAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostWithCommentsQuery query)
        {
            return await _dbContext.Posts.Include(x => x.Comments).Where(x => x.Comments.Any()).AsNoTracking().ToListAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByAuthorQuery query)
        {
            return await _dbContext.Posts.Include(x => x.Comments).Where(x => x.Author.ToLower() == query.Author.Trim().ToLower()).AsNoTracking().ToListAsync();
        }
    }
}

