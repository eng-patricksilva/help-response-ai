using HelpResponse.AI.Domain.Conversations.Requests;
using HelpResponse.AI.Domain.Conversations.Responses;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HelpResponse.AI.Services.Conversations
{
    public class ConversationService(ILogger<ConversationService> looger) : IConversationService
    {
        private readonly ILogger<ConversationService> _logger = looger;

        public async Task<ConversationResponse> CreateConversation(ConversationRequest request)
        {
            await Task.Delay(10);

            //var challenge = new ChallengeDomain(request.Id);
            //_logger.LogInformation("Challenge created successfully. {@challenge}", challenge);

            //var response = _mapper.Map<ConversationResponse>(challenge);

            //return response;

            return new ConversationResponse();
        }
    }
}