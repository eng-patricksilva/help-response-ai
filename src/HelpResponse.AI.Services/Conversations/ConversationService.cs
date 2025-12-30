using HelpResponse.AI.Domain.Conversations.Requests;
using HelpResponse.AI.Domain.Conversations.Responses;
using HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp;
using HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp.Configuration;
using HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp.Searches;
using HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp;
using HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp.Configuration;
using HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp.Embeddings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HelpResponse.AI.Services.Conversations
{
    public class ConversationService(ILogger<ConversationService> logger,
                                     IOpenIAClientApi openAIClientApi,
                                     IClaudiaDbClientApi claudiaDbClientApi,
                                     IOptions<OpenAiApi> openAiOption,
                                     IOptions<ClaudiaDbApi> claudiaDbOption) : IConversationService
    {
        private readonly OpenAiApi _openAiApi = openAiOption.Value;
        private readonly ClaudiaDbApi _claudiaDbApi = claudiaDbOption.Value;

        public async Task<ConversationResponse> CreateConversation(ConversationRequest request)
        {
            var outputEmbedding = await Embed(request);

            var outputVector = await AddVector(outputEmbedding.Data.LastOrDefault()?.Embedding ?? []);

            //var response = _mapper.Map<ConversationResponse>(challenge);

            //return response;

            return new ConversationResponse();
        }

        private async Task<EmbeddingOutput> Embed(ConversationRequest request)
        {
            try
            {
                var embedding = new EmbeddingInput(request.Messages.LastOrDefault()?.Content ?? string.Empty, _openAiApi.Model);
                return await openAIClientApi.SendEmbeddings(embedding);
            }
            catch (Refit.ApiException apiEx)
            {
                logger.LogError(apiEx, "HTTP error occurred while sending embedding: {StatusCode} - {ResponseContent}", apiEx.StatusCode, apiEx.Content);
                throw new InvalidOperationException(apiEx.Content);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while sending embedding");
                throw new InvalidOperationException("Failed to get embedding from OpenAI API", ex);
            }
        }

        private async Task<SearchOutput> AddVector(float[] vector)
        {
            try
            {
                if (vector.Length == 0)
                    throw new ArgumentException("Vector cannot be empty", nameof(vector));

                var searchInput = new SearchInput(_claudiaDbApi.Count,
                                                  _claudiaDbApi.Select,
                                                  _claudiaDbApi.Top,
                                                  _claudiaDbApi.Filter,
                                                  vector,
                                                  _claudiaDbApi.Fields,
                                                  _claudiaDbApi.Kind);

                return await claudiaDbClientApi.Search(searchInput);
            }
            catch (Refit.ApiException apiEx)
            {
                logger.LogError(apiEx, "HTTP error occurred while sending embedding: {StatusCode} - {ResponseContent}", apiEx.StatusCode, apiEx.Content);
                throw new InvalidOperationException("Failed to get embedding from OpenAI API", apiEx);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while sending embedding");
                throw new InvalidOperationException("Failed to get embedding from OpenAI API", ex);
            }
        }
    }
}