using DomainLayer.Entities;

namespace Api.Queries
{
	public interface IQueryHandler
	{
		Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query);
        Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostWithCommentsQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostByAuthorQuery query);
    }
}

