using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.WebUI.Configuration.BuilderServices;
using NLayerArchTemplate.Business;
using NLayerArchTemplate.WebUI.Configuration.Converter;

namespace NLayerArchTemplate.WebUI.BuilderServices;

public static class BuilderService
{
    public static void Create(this IServiceCollection services,
                              IConfiguration configuration,
                              bool isDevelopment,
                              string connectionString)
    {
        // Add services to the container.
        services.AddControllersWithViews(options =>
        {
            //Session ve yetki kontolü yapılıyor.
            options.AddAuthorizationFilter(configuration);
            //HttpPost,HttpPatch gibi işlemlerde token doğrulaması yapar.(ClickJacking saldırısını önlemek için gerekli.)
            options.Filters.Add(typeof(AutoValidateAntiforgeryTokenAttribute));
        }).AddRazorRuntimeCompilation();//cshtml files will be compiled on runtime
        
        services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new StringToBoolConverter());
            options.JsonSerializerOptions.WriteIndented = true; // Veriyi okunabilir şekilde formatlamak için
            options.JsonSerializerOptions.PropertyNamingPolicy = null; // JsonNamingPolicy.PascalCase için
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.IncludeFields = true;
        });

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
