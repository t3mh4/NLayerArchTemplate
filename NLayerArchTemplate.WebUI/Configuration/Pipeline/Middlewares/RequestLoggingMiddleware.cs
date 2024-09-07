using System.Text;

namespace NLayerArchTemplate.WebUI.Configuration.Pipeline.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log request body
        context.Request.EnableBuffering(); // Enable buffering to read the request body
        var buffer = new byte[context.Request.ContentLength ?? 0];
        await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
        var requestBody = Encoding.UTF8.GetString(buffer);
        // Log the request body (you can use your preferred logging mechanism)
        Console.WriteLine("Request Body : " + requestBody);
        // Reset the body stream position so the rest of the pipeline can read it
        context.Request.Body.Position = 0;
        await _next(context);
    }
}
