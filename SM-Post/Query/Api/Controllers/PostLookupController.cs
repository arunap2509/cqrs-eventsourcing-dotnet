using Api.DTOs;
using Api.Queries;
using CQRS.Core.Infrastructure;
using DomainLayer.Entities;
using InfrastructureLayer.DataAccess;
using Microsoft.AspNetCore.Mvc;
using PostCommon.DTOs;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/v1/post-lookup")]
	public class PostLookupController: ControllerBase
	{
		private readonly ILogger<PostLookupController> _logger;
		private readonly IQueryDispatcher<PostEntity> _queryDispatcher;
        private readonly DatabaseContext _dbContext;

        public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> queryDispatcher, DatabaseContext dbContext)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPostsAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindAllPostsQuery());

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"successfully returned {posts.Count} records"
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "error while processing request");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = "error while processing request"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPostsByIdAsync(Guid id)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = id });

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"successfully returned {posts.Count} records"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while processing request");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = "error while processing request"
                });
            }
        }

        [HttpGet("byLikes/{numberOfLikes}")]
        public async Task<ActionResult> GetPostsByLikesAsync(int numberOfLikes)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery { NumberOfLikes = numberOfLikes });

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"successfully returned {posts.Count} records"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while processing request");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = "error while processing request"
                });
            }
        }

        [HttpGet("byAuthor/{author}")]
        public async Task<ActionResult> GetPostsByAuthorAsync(string author)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByAuthorQuery { Author = author });

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"successfully returned {posts.Count} records"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while processing request");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = "error while processing request"
                });
            }
        }

        [HttpGet("byComment")]
        public async Task<ActionResult> GetPostsWithCommentAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostWithCommentsQuery());

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"successfully returned {posts.Count} records"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while processing request");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = "error while processing request"
                });
            }
        }
    }
}

