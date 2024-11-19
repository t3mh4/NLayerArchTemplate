using Microsoft.Net.Http.Headers;

namespace NLayerArchTemplate.WebUI.Configuration.Middlewares;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _environment;


    public SecurityHeadersMiddleware(RequestDelegate next, IWebHostEnvironment environment = null)
    {
        _next = next;
        _environment = environment;
    }

    public async Task Invoke(HttpContext context)
    {
        if (_environment.IsProduction())
        {
            //to disallow display your website inside an iframe
            context.Response.Headers[HeaderNames.XFrameOptions] = "DENY";
            //to disallow to inject JavaScript into an svg file.
            context.Response.Headers[HeaderNames.XContentTypeOptions] = "nosniff";
            //to not allow browsers to display your website as last visited in “Referer” header
            context.Response.Headers["Referrer-Policy"] = "no-referrer";//
            //disable the possibility of Flash making cross-site requests
            context.Response.Headers["X-Permitted-Cross-Domain-Policies"] = "none";
            // to stop loading the page when a cross-site scripting attack is identified. 
            context.Response.Headers[HeaderNames.XXSSProtection] = "1; mode=block";
            // tells the browser which platform features your website needs
            context.Response.Headers["Permissions-Policy"] = "camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), usb=()";
            // Configure the Content Security Policy (CSP) header
            //context.Response.Headers[HeaderNames.ContentSecurityPolicy] =
            //    "default-src 'self'; " +
            //    "img-src 'self' data:; " +
            //    "script-src 'self'; " +
            //    "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
            //    "font-src 'self' https://fonts.gstatic.com; " +
            //    "form-action 'self'; " +
            //    "frame-ancestors 'self';";
        }
        await _next.Invoke(context);
    }
}