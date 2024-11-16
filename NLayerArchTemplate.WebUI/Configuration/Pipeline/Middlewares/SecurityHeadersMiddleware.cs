using Microsoft.Extensions.Primitives;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.Core.ConstantMessages;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Core.Models;

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
            context.Response.Headers["Server"] = string.Empty;

            //to disallow display your website inside an iframe
            context.Response.Headers["X-Frame-Options"] = "DENY";

            //to disallow to inject JavaScript into an svg file.
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            //to not allow browsers to display your website as last visited in “Referer” header
            context.Response.Headers["Referrer-Policy"] = "no-referrer";//
            //disable the possibility of Flash making cross-site requests
            context.Response.Headers["X-Permitted-Cross-Domain-Policies"] = "none";
            // to stop loading the page when a cross-site scripting attack is identified. 
            context.Response.Headers["X-Xss-Protection"] = "1; mode=block";

            // tells the browser which platform features your website needs
            context.Response.Headers["Permissions-Policy"] = "camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), usb=()";

            // Configure the Content Security Policy (CSP) header
            context.Response.Headers["Content-Security-Policy"] =
                "default-src 'self'; " +
                "img-src 'self' data:; " +
                "script-src 'self'; " +
                "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
                "font-src 'self' https://fonts.gstatic.com; " +
                "form-action 'self'; " +
                "frame-ancestors 'self';";
        }
        await _next.Invoke(context);
    }
}