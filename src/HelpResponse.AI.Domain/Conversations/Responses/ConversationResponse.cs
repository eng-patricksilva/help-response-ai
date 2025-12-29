using System.Collections.Generic;

namespace HelpResponse.AI.Domain.Conversations.Responses
{
    public class ConversationResponse
    {
        public IEnumerable<ConversationMessageResponse> Messages { get; set; }
        public bool HandoverToHumanNeeded { get; set; }
        public IEnumerable<SectionsRetrieved> SectionsRetrieved { get; set; }
    }

    public class ConversationMessageResponse
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public class SectionsRetrieved
    {
        public float Score { get; set; }
        public string Content { get; set; }
    }
}