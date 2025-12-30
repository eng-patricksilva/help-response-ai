using System.Collections.Generic;

namespace HelpResponse.AI.Domain.Conversations.Responses
{
    public class ConversationResponse
    {
        public List<ConversationMessageResponse> Messages { get; set; }
        public bool HandoverToHumanNeeded { get; set; }
        public List<SectionsRetrieved> SectionsRetrieved { get; set; }

        public static ConversationResponse Create() => new();

        public ConversationResponse AddMessage(string content, string role)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new System.ArgumentException("Message content cannot be null or whitespace.", nameof(content));
            if (string.IsNullOrWhiteSpace(role))
                throw new System.ArgumentException("Message role cannot be null or whitespace.", nameof(role));

            Messages ??= [];
            Messages.Add(new ConversationMessageResponse
            {
                Role = role.ToUpper(),
                Content = content
            });
            return this;
        }

        public ConversationResponse AddRetrieved(float score, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new System.ArgumentException("Section content cannot be null or whitespace.", nameof(content));
            if (score < 0)
                throw new System.ArgumentOutOfRangeException(nameof(score), "Score cannot be negative.");

            SectionsRetrieved ??= [];
            SectionsRetrieved.Add(new SectionsRetrieved
            {
                Score = score,
                Content = content
            });
            return this;
        }
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