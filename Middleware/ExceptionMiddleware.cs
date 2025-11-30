using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TeamHeartFiap.Middleware;

public class ExceptionMiddlewareLegacy
{
    private readonly RequestDelegate _next;

    public ExceptionMiddlewareLegacy(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected error occurred.");
        }
    }
}
