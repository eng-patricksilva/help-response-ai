using System.Text.Json.Serialization;

namespace HelpResponse.AI.Domain.Responses
{
    public class ApplicationExceptionResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("detail")]
        public string Detail { get; set; }
    }
}