using Api.Commands;
using Api.DTOs;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using PostCommon.DTOs;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/restore")]
    public class RestoreReadDbController : ControllerBase
	{
        private readonly ILogger<RestoreReadDbController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public RestoreReadDbController(ILogger<RestoreReadDbController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> NewPostAsync()
        {
            try
            {
                await _commandDispatcher.SendAsync(new RestoreReadDbCommand());

                return Ok(new NewPostReponse
                {
                    Message = "restore post request completed successfully"
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
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = "error while parsing request"
                });
            }
        }
    }
}

