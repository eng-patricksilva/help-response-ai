using HelpResponse.AI.Domain.Bases;
using HelpResponse.AI.Domain.Challenges.Requests;
using HelpResponse.AI.Domain.Challenges.Responses;
using HelpResponse.AI.Domain.Responses;
using HelpResponse.AI.Services.Challenge;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpResponse.AI.WebApi.Controllers.v1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ChallengeController(IChallengeService challengeService, ILogger<ChallengeController> logger) : ControllerBase
    {
        [HttpGet("all")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ChallengeResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApplicationExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            return await ExecuteAsync(() => challengeService.GetChallenges());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ChallengeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApplicationExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetChallenge(int id)
        {
            return await ExecuteAsync(() => challengeService.GetChallenge(id));
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(BaseResponse<ChallengeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApplicationExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApplicationExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateChallenge([FromBody] ChallengeRequest request)
        {
            return await ExecuteAsync(() =>
            {
                logger.LogInformation("Creating challenge with Id: {Id}", request.Id);

                return challengeService.CreateChallenge(request);
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
