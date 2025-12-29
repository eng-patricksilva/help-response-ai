using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace HelpResponse.AI.WebApi.Filters;

public class GlobalResponseFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult && objectResult.StatusCode is null or >= 200 and < 300)
        {
            var wrapped = new
            {
                data = objectResult.Value
            };

            context.Result = new ObjectResult(wrapped)
            {
                StatusCode = objectResult.StatusCode
            };
        }

        await next();
    }
}