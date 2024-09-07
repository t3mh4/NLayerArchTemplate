using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Newtonsoft.Json;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.Core.ConstantMessages;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.WebUI.Configuration.Middlewares;
using NLayerArchTemplate.WebUI.Configuration.Pipeline.Middlewares;
using System.Globalization;

namespace NLayerArchTemplate.WebUI.Configuration.Pipeline;

/* ORDER OF THE MIDDLEWARES
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCookiePolicy();
    app.UseRouting();
    app.UseRateLimiter();
    app.UseRequestLocalization();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSession();
    app.UseResponseCompression();
    app.UseResponseCaching();
    **Custom Middleware can be added here
    app.MapRazorPages();
    app.MapDefaultControllerRoute();
 */
public static class Pipeline
{
    public static void CreatePipeline(this IApplicationBuilder app, bool isDevelopment
    {
        app.UseMiddleware<SecurityHeadersMiddleware>(); 
        if (isDevelopment)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
        }
        //----------------
        //For activating Strict-Transport-Security - web security policy mechanism that helps to
        //protect your website from protocol downgrade attacks and cookie hijacking,
        //add the next one to your middleware pipeline (or just don’t remove it),
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseExceptionHandler("/Error/Handle");
        app.UseStaticFiles();
        //app.UseCookiePolicy();
        app.UseRouting();
        //app.UseRateLimiter();
        //tarih,sayılar için türkçe formatını ayarlıyoruz
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("tr-TR"),
            SupportedCultures = [new CultureInfo("tr-TR")],
            SupportedUICultures = [new CultureInfo("tr-TR")]
        });
        //-------------------
        //app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        //app.UseSession();
        //app.UseResponseCompression();iptal ettim çünkü güvenlik zafiyeti oluşturabilirmiş.
        //app.UseResponseCaching();
        app.UseHealthChecks("/health");
	    app.UseStatusCodePages(context =>
		{
			var request = context.HttpContext.Request;
			var response = context.HttpContext.Response;
			var originalPath = request.Path;
			var originalQueryString = request.QueryString;
			var redirectUrl = $"/Error/{response.StatusCode}?originalPath={originalPath}&originalQueryString={originalQueryString}";
			if (request.Headers.Any(a => a.Key == KeyValues.XRequestedWith))
			{
				var errorModel = new ErrorModel
				{
					Message = ErrorMessages.HataliIslem,
				};
				response.ContentType = KeyValues.JsonContentType;
				var responseModel = HttpResponseModel<ErrorModel>.Fail(errorModel);
				response.WriteAsync(responseModel.ToJSON()).Wait();
				return Task.CompletedTask;
			}
			response.Redirect(redirectUrl);
			return Task.CompletedTask;
		});
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}");
            endpoints.MapHealthChecks("/health");
        });
    }
}
