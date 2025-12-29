using HelpResponse.AI.Services.Conversations;
using Microsoft.Extensions.DependencyInjection;

namespace HelpResponse.AI.Services;

public static class ServicesExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IConversationService, ConversationService>();
    }
}