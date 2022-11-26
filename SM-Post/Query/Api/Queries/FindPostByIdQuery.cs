using CQRS.Core.Queries;

namespace Api.Queries
{
	public class FindPostByIdQuery: BaseQuery
	{
		public Guid Id { get; set; }
	}
}

