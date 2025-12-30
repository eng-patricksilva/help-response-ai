using System.Text.Json.Serialization;

namespace HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp.Embeddings
{
    public record EmbeddingInput(
        [property: JsonPropertyName("input")] string Input,
        [property: JsonPropertyName("model")] string Model
    );
}