using HelpResponse.AI.Domain.Conversations.Requests;
using HelpResponse.AI.Domain.Conversations.Responses;
using HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp;
using HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp.Configuration;
using HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp.Searches;
using HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp;
using HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp.ChatCompletions;
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
        private const string USER = "user";
        private const string SYSTEM = "system";

        private readonly OpenAiApi _openAiApi = openAiOption.Value;
        private readonly ClaudiaDbApi _claudiaDbApi = claudiaDbOption.Value;

        public async Task<ConversationResponse> CreateConversation(ConversationRequest request)
        {
            try
            {
                var inputEmb = new EmbeddingInput(request.Message.Content ?? string.Empty, _openAiApi.Model);
                var outputEmbedding = await openAIClientApi.SendEmbeddings(inputEmb);

                var inputVector = BuildVector(outputEmbedding.Data.LastOrDefault()?.Embedding ?? []);
                var outputVector = await claudiaDbClientApi.Search(inputVector);

                var contentMaxScore = outputVector.Value.OrderByDescending(x => x.Searchscore).FirstOrDefault();

                var inputChat = BuildChat(contentMaxScore.Content, inputEmb.Input);
                var outputChat = await openAIClientApi.SendChatCompletions(inputChat);

                return BuildResponse(outputVector, inputChat, outputChat);
            }
            catch (Refit.ApiException apiEx)
            {
                logger.LogError(apiEx, "HTTP error occurred while sending apis: {StatusCode} - {ResponseContent}", apiEx.StatusCode, apiEx.Content);
                throw new InvalidOperationException(apiEx.Content);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while sending apis");
                throw new InvalidOperationException(ex.Message);
            }
        }

        private static ConversationResponse BuildResponse(SearchOutput outputVector, ChatCompletionInput inputChat, ChatCompletionOutput outputChat)
        {
            var inputUserMessage = inputChat.Messages.FirstOrDefault(x => x.Role == USER);
            var choicesMessage = outputChat.Choices.FirstOrDefault();

            var response = ConversationResponse.Create()
                                               .AddMessage(inputUserMessage.Content, inputUserMessage.Role)
                                               .AddMessage(choicesMessage?.Message.Content, choicesMessage.Message.Role);

            foreach (var item in outputVector.Value)
                response = response.AddRetrieved(item.Searchscore, item.Content);
            return response;
        }

        private ChatCompletionInput BuildChat(string message, string input)
        {
            var system = _openAiApi.PromptTemplates.FirstOrDefault(x => x.Rule == SYSTEM);
            var user = _openAiApi.PromptTemplates.FirstOrDefault(x => x.Rule == USER);

            return new ChatCompletionInput
            {
                Model = _openAiApi.ChatCompletionModel,
                Messages =
                [
                    new MessageInput(system.Rule, system.Message, message),
                    new MessageInput(user.Rule, user.Message, input)
                ]
            };
        }

        private SearchInput BuildVector(float[] vector)
        {
            if (vector.Length == 0)
                throw new ArgumentException("Vector cannot be empty", nameof(vector));

            return new SearchInput(vector,
                                   _claudiaDbApi.Count,
                                   _claudiaDbApi.Select,
                                   _claudiaDbApi.Top,
                                   _claudiaDbApi.Filter,
                                   _claudiaDbApi.Fields,
                                   _claudiaDbApi.Kind);
        }
    }
}