using HelpResponse.AI.Domain.Settings;
using HelpResponse.AI.Services;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HelpResponse.AI.WebApi.IoC;

public static class StartupExtensions
{
    public static void ConfigureServices(this IServiceCollection services, ApiConfiguration apiSettings)
    {
        ArgumentNullException.ThrowIfNull(apiSettings?.DefaultRequestTimeout);

        services.AddRequestTimeouts(options =>
        {
            options.DefaultPolicy = new RequestTimeoutPolicy
            {
                Timeout = TimeSpan.FromSeconds(apiSettings.DefaultRequestTimeout),
                TimeoutStatusCode = 504
            };
        });

        services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new LowercaseTransformer()));
        }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        });

        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(apiSettings.GetMajorVersion(), apiSettings.GetMinorVersion());
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerExtension(apiSettings);

        //services.AddAutoMapper(typeof(ConversationMapperProfile));

        services.RegisterServices();

        services.AddHealthChecks();
    }
}

public class LowercaseTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object value) => value?.ToString()?.ToLower();
}