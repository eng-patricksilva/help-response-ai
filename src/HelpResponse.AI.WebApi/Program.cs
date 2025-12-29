using HelpResponse.AI.Domain.Settings;
using HelpResponse.AI.WebApi.IoC;
using HelpResponse.AI.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
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

var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiConfiguration>();

builder.Services.ConfigureServices(apiSettings);

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