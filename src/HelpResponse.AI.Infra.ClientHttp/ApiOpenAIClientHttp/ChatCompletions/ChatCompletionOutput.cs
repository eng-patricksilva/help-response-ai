using System.Collections.Generic;

namespace HelpResponse.AI.Infra.ClientHttp.ApiOpenAIClientHttp.ChatCompletions
{
    public class ChatCompletionOutput
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public int Created { get; set; }
        public string Model { get; set; }
        public IEnumerable<Choice> Choices { get; set; }
        public Usage Usage { get; set; }
        public string ServiceTier { get; set; }
        public string SystemFingerprint { get; set; }
    }

    public class Usage
    {
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
        public PromptTokensDetails PromptTokensDetails { get; set; }
        public CompletionTokensDetails CompletionTokensDetails { get; set; }
    }

    public class PromptTokensDetails
    {
        public int CachedTokens { get; set; }
        public int AudioTokens { get; set; }
    }

    public class CompletionTokensDetails
    {
        public int ReasoningTokens { get; set; }
        public int AudioTokens { get; set; }
        public int AcceptedPredictionTokens { get; set; }
        public int RejectedPredictionTokens { get; set; }
    }

    public class Choice
    {
        public int Index { get; set; }
        public MessageOutput Message { get; set; }
        public object Logprobs { get; set; }
        public string FinishReason { get; set; }
    }

    public class MessageOutput
    {
        public string Role { get; set; }
        public string Content { get; set; }
        public object Refusal { get; set; }
        public object[] Annotations { get; set; }
    }
}
