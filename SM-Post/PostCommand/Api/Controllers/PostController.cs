using Api.Commands;
using Api.DTOs;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using PostCommon.DTOs;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/post")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public PostController(ILogger<PostController> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<ActionResult> NewPostAsync(NewPostCommand command)
    {
        var id = Guid.NewGuid();

        try
        {
            command.Id = id;

            await _commandDispatcher.SendAsync(command);

            return Ok(new NewPostReponse
            {
                Message = "new post creation request completed successfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "bad request");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, "error while parsing request");
            return StatusCode(StatusCodes.Status500InternalServerError, new NewPostReponse
            {
                Id = id,
                Message = "error while parsing request"
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> EditPostAsync([FromRoute] Guid id, EditPostCommand command)
    {
        try
        {
            command.Id = id;
            await _commandDispatcher.SendAsync(command);

            return Ok(new BaseResponse
            {
                Message = "edit message completed successfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "bad request");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "could not retreive aggregate client passed an incorrect post id");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, "error while parsing request");
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = "error while parsing request"
            });
        }
    }

    [HttpPut("byLikes/{id}")]
    public async Task<ActionResult> LikePostAsync(Guid id)
    {
        try
        {
            await _commandDispatcher.SendAsync(new LikePostCommand { Id = id });

            return Ok(new BaseResponse
            {
                Message = "like post completed successfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "bad request");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "could not retreive aggregate client passed an incorrect post id");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, "error while parsing request while like");
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = "error while parsing request"
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePostAsync(Guid id, DeletePostCommand command)
    {
        try
        {
            command.Id = id;
            await _commandDispatcher.SendAsync(command);

            return Ok(new BaseResponse
            {
                Message = "remove post completed successfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "bad request");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "could not retreive aggregate client passed an incorrect post id");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, "error while parsing request");
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = "error while parsing request"
            });
        }
    }
}

