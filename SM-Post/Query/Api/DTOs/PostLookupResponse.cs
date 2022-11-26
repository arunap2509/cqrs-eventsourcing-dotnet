using DomainLayer.Entities;
using PostCommon.DTOs;

namespace Api.DTOs
{
	public class PostLookupResponse : BaseResponse
	{
		public List<PostEntity> Posts { get; set; }
	}
}

