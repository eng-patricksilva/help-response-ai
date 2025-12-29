namespace HelpResponse.AI.Infra.ClientHttp.ApiOpenAIClientHttp.Embeddings
{
    public class EmbeddingOutput
    {
        public string Object { get; set; }
        public DataContent[] Data { get; set; }
        public string Model { get; set; }
        public Usage Usage { get; set; }
    }

    public class Usage
    {
        public int PromptTokens { get; set; }
        public int TotalTokens { get; set; }
    }

    public class DataContent
    {
        public string Object { get; set; }
        public int Index { get; set; }
        public float[] Embedding { get; set; }
    }
}