using HelpResponse.AI.Services.Challenge;
using Microsoft.Extensions.DependencyInjection;

namespace HelpResponse.AI.Services;

public static class ServicesExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IChallengeService, ChallengeService>();
    }
}