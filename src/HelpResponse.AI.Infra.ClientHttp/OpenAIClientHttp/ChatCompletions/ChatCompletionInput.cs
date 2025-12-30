using System.Collections.Generic;

namespace HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp.ChatCompletions
{
    public class ChatCompletionInput
    {
        public string Model { get; set; }
        public IEnumerable<MessageInput> Messages { get; set; }
    }

    public class MessageInput(string role, string template, string message)
    {
        public string Role { get; set; } = role;
        public string Content { get; set; } = string.Format(template, message);
    }
}