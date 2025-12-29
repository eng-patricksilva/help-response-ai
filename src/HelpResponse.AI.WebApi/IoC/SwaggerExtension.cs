using HelpResponse.AI.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace HelpResponse.AI.WebApi.IoC;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services, ApiConfiguration apiSettings)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = apiSettings.Title,
                Description = apiSettings.Description,
                Version = $"v{apiSettings.Version}"
            });

            var xmlFile = $"HelpResponse.AI.WebApi.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath, true);
        });

        services.AddSwaggerGenNewtonsoftSupport();

        return services;
    }
}