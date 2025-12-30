namespace HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp.Configuration
{
    public class OpenAiApi
    {
        public string Url { get; set; }
        public string Token { get; set; }
        public string Model { get; set; }
        public string ChatCompletionModel { get; set; }
        public PromptTemplate[] PromptTemplates { get; set; }

        public class PromptTemplate
        {
            public string Message { get; set; }
            public string Rule { get; set; }
        }
    }
}