using CQRS.Core.Queries;

namespace Api.Queries
{
	public class FindPostsWithLikesQuery : BaseQuery
	{
		public int NumberOfLikes { get; set; }
	}
}

