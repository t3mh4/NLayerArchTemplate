using Microsoft.AspNetCore.Localization;
using NLayerArchTemplate.WebUI.Configuration.Middlewares;
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
    public static void CreatePipeline(this IApplicationBuilder app)
    {
        //oluşan hataları yakalayıp ErrorController'a yönlendiriyor.
        app.UseExceptionHandler("/Error/Handle");
        app.UseStatusCodePagesWithReExecute("/Error/Index", "?code={0}");
        app.UseMiddleware<SecurityHeadersMiddleware>();
        //----------------
        //For activating Strict-Transport-Security - web security policy mechanism that helps to
        //protect your website from protocol downgrade attacks and cookie hijacking,
        //add the next one to your middleware pipeline (or just don’t remove it),
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        //app.UseCookiePolicy();
        app.UseRouting();
        //app.UseRateLimiter();
        //tarih,sayılar için türkçe formatını ayarlıyoruz
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("tr-TR"),
            SupportedCultures = new[] { new CultureInfo("tr-TR") },
            SupportedUICultures = new[] { new CultureInfo("tr-TR") }
        });
        //-------------------
        //app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        //app.UseSession();
        //app.UseResponseCompression();iptal ettim çünkü güvenlik zafiyeti oluşturabilirmiş.
        //app.UseResponseCaching();
        //app.UseMiddleware<ErrorHandlerMiddleware>();//.Net Core API projelerinde global error handler middleware kullanmak daha doğru olur.
        app.UseHealthChecks("/health");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}");
            endpoints.MapHealthChecks("/health");
        });
    }
}
