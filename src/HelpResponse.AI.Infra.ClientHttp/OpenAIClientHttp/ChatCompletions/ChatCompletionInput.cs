using System.Collections.Generic;

namespace HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp.ChatCompletions
{
    public class ChatCompletionInput
    {
        public string Model { get; set; }
        public IEnumerable<MessageInput> Messages { get; set; }
    }

    public class MessageInput
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}