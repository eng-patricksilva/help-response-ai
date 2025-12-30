using HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp;
using HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp.Configuration;
using HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp;
using HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace HelpResponse.AI.Infra.ClientHttp
{
    public static class ClientsHttpExtensions
    {
        private static OpenAiApi _openAIConfig;
        private static ClaudiaDbApi _claudiaDbConfig;

        public static void RegisterClientHttp(this IServiceCollection services, IConfiguration configuration)
        {
            _openAIConfig = configuration.GetSection("ClientIntegrations:OpenAIApi").Get<OpenAiApi>();
            _claudiaDbConfig = configuration.GetSection("ClientIntegrations:ClaudiaDbApi").Get<ClaudiaDbApi>();

            services.AddRefitClient<IOpenIAClientApi>()
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri(_openAIConfig.Url);
                    })
                    .AddHttpMessageHandler(() => new AuthOpenIAClienHandler());

            services.AddRefitClient<IClaudiaDbClientApi>()
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri(_claudiaDbConfig.Url);
                    })
                    .AddHttpMessageHandler(() => new AuthClaudiaDbClientHandler());
        }

        public class AuthOpenIAClienHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _openAIConfig.Token);

                return base.SendAsync(request, cancellationToken);
            }
        }

        public class AuthClaudiaDbClientHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Add("api-key", _claudiaDbConfig.Token);

                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}