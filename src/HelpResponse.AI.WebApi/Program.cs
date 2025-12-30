using HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp.Configuration;
using HelpResponse.AI.Infra.ClientHttp.OpenAIClientHttp.Configuration;
using HelpResponse.AI.WebApi.IoC;
using HelpResponse.AI.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

string environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
_ = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json")
                              .AddJsonFile("appsettings." + environmentVariable + ".json", optional: true)
                              .AddEnvironmentVariables()
                              .Build();

builder.Logging.AddSerilog(builder.Configuration);

builder.Services.Configure<OpenAiApi>(builder.Configuration.GetSection("ClientIntegrations:OpenAIApi"));
builder.Services.Configure<ClaudiaDbApi>(builder.Configuration.GetSection("ClientIntegrations:ClaudiaDbApi"));

var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiConfiguration>();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

string pathBase = apiSettings.GetPathBase();
app.UsePathBase(pathBase);
app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    int majorVersion = apiSettings.GetMajorVersion();
    c.SwaggerEndpoint($"{pathBase}/swagger/v{majorVersion}/swagger.json", $"v{majorVersion}");
    c.RoutePrefix = "swagger";
});

app.UseRequestTimeouts();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<LogCorrelationIdMiddleware>();
app.UseCors("cors");
app.UseHealthChecks("/health-check");
app.MapControllers();

try
{
    app.Run();
}
catch (Exception exception)
{
    Log.ForContext(typeof(WebApplication)).Fatal(exception, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}