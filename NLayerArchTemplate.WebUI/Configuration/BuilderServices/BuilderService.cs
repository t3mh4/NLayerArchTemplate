using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.Core.Settings;
using NLayerArchTemplate.WebUI.Configuration.BuilderServices;
using NLayerArchTemplate.Business;

namespace NLayerArchTemplate.WebUI.BuilderServices;

public static class BuilderService
{
    public static void Create(this IServiceCollection services,
                              IConfiguration configuration,
                              bool isDevelopment,
                              string connectionString)
    {
        var jsonSettings = new CustomJsonSerializerSettings();

        // Add services to the container.
        services.AddControllersWithViews(options =>
        {
            //Session ve yetki kontolü yapılıyor.
            options.AddAuthorizationFilter(configuration);
            //HttpPost,HttpPatch gibi işlemlerde token doğrulaması yapar.(ClickJacking saldırısını önlemek için gerekli.)
            options.Filters.Add(typeof(AutoValidateAntiforgeryTokenAttribute));
        }).AddNewtonsoftJson(options => //System.Text.Json yerine Newtonsoft'u kullanabilmek için eklendi.
        {
            options.SerializerSettings.ContractResolver = jsonSettings.ContractResolver;
            options.SerializerSettings.ReferenceLoopHandling = jsonSettings.ReferenceLoopHandling;
            options.SerializerSettings.DateFormatString = jsonSettings.DateFormatString;
            options.SerializerSettings.Formatting = jsonSettings.Formatting;
        }).AddRazorRuntimeCompilation();//cshtml files will be compiled on runtime


        //Business katmanında kullanılacak DI'lar içn eklendi.
        services.AddBusinessServices(isDevelopment, connectionString);

        //builder.Services.AddResponseCompressionService(); iptal ettim çünkü güvenlik zafiyeti oluşturabilirmiş.

        //Yetkilendirme işlemleri
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                //Authorize attribute ile kullanıcı login olmadan erişmek istediğinde yönlendirileceği sayfa
                //CustomAuthorize attribute kullanıldığı için bu ayar yapılmadı.
                //options.LoginPath = "/Account/Login";
                //options.LogoutPath = "/Account/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(KeyValues.CookieTimeOut);
            });

        services.AddHealthChecks();
        //Antiforgery token yeniden düzenlendi.
        services.AddAntiforgery(options =>
        {
            options.FormFieldName = "RequestVerificationToken";
            options.SuppressXFrameOptionsHeader = true;
            options.HeaderName = "X-HttpRequest-Token";
        });
    }
}
