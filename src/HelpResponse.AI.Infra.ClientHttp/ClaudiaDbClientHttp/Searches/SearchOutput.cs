using System.Text.Json.Serialization;

namespace HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp.Searches
{
    public class SearchOutput
    {
        [JsonPropertyName("@odata.context")]
        public string Odatacontext { get; set; }
        [JsonPropertyName("@odata.count")]
        public int Odatacount { get; set; }
        public ArrayValue[] Value { get; set; }
    }

    public class ArrayValue
    {
        [JsonPropertyName("@search.score")]
        public float Searchscore { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
    }
}