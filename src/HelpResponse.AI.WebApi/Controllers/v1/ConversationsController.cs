using HelpResponse.AI.Domain.Conversations.Requests;
using HelpResponse.AI.Domain.Conversations.Responses;
using HelpResponse.AI.Domain.Responses;
using HelpResponse.AI.Services.Conversations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HelpResponse.AI.WebApi.Controllers.v1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ConversationsController(IConversationService conversationService, ILogger<ConversationsController> logger) : ControllerBase
    {
        [HttpPost("completions")]
        [ProducesResponseType(typeof(ConversationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApplicationExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateConversation([FromBody] ConversationRequest request)
        {
            return await ExecuteAsync(() =>
            {
                logger.LogInformation("Creating conversation with Id: {Id}", 1);

                return conversationService.CreateConversation(request);
            });
        }

        private async Task<IActionResult> ExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action();
                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                logger.LogError("Application Time Out");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unespected Error");

                return new ObjectResult(new { message = "Internal Error", detail = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
