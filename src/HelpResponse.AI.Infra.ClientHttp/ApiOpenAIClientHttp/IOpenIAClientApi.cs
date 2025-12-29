using HelpResponse.AI.Infra.ClientHttp.ApiOpenAIClientHttp.ChatCompletions;
using HelpResponse.AI.Infra.ClientHttp.ApiOpenAIClientHttp.Embeddings;
using Refit;
using System.Threading.Tasks;

namespace HelpResponse.AI.Infra.ClientHttp.ApiOpenAIClientHttp
{
    public interface IOpenIAClientApi
    {
        [Post("/v1/embeddings")]
        Task<EmbeddingOutput> SendEmbeddings([Body] EmbeddingInput input);

        [Post("/v1/chat/completions")]
        Task<ChatCompletionOutput> SendChatCompletions([Body] ChatCompletionInput input);
    }
}