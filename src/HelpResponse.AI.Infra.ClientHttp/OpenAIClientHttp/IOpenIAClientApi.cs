using HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp.ChatCompletions;
using HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp.Embeddings;
using Refit;
using System.Threading.Tasks;

namespace HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp
{
    public interface IOpenIAClientApi
    {
        [Post("/v1/embeddings")]
        Task<EmbeddingOutput> SendEmbeddings([Body] EmbeddingInput input);

        [Post("/v1/chat/completions")]
        Task<ChatCompletionOutput> SendChatCompletions([Body] ChatCompletionInput input);
    }
}