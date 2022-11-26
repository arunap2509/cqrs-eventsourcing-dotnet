using CQRS.Core.Queries;

namespace Api.Queries
{
	public class FindPostByAuthorQuery : BaseQuery
	{
		public string Author { get; set; }
	}
}

