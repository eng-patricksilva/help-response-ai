using System.Collections.Generic;

namespace HelpResponse.AI.Domain.Conversations.Requests
{
    public class ConversationRequest
    {
        public int HelpdeskId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<ConversationMessageRequest> Messages { get; set; }
    }

    public class ConversationMessageRequest
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}