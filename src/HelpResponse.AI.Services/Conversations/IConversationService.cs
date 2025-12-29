using HelpResponse.AI.Domain.Conversations.Requests;
using HelpResponse.AI.Domain.Conversations.Responses;
using System.Threading.Tasks;

namespace HelpResponse.AI.Services.Conversations
{
    public interface IConversationService
    {
        public Task<ConversationResponse> CreateConversation(ConversationRequest request);
    }
}