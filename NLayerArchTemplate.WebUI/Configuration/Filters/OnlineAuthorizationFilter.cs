using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.WebUI.Configuration.ActionResults;
using NLayerArchTemplate.WebUI.Configuration.Extensions;
using NLayerArchTemplate.WebUI.Helpers;
using NtierArchTemplate.Business.UserManager;
using System.Net;

namespace NLayerArchTemplate.WebUI.Configuration.Filters;
public sealed class OnlineAuthorizationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {

        var httpContext = context.HttpContext;
        var httpContextRequest = httpContext.Request;
        //ajax requestler axios ile yapılıp yapılmadığı kontrol ediliyor.
        if (!CheckIfRequestIsValid(httpContextRequest))
        {
            var statusCode = HttpStatusCode.BadRequest.ToInt32();
            var resModel = HttpResponseModel.Fail("Hatalı İşlem..!!");
            context.Result = new JsonActionResult(resModel, statusCode);
            return;
        }
        if (IsAllowedAnonymous(context) || await IsUserAuthenticated(httpContext)) return;
        var queryString = httpContextRequest.QueryString.ToString();
        var path = httpContextRequest.Path.Value;
        var returnUrl = string.Concat("?ReturnUrl=", path, queryString);
        //refresh yapılan sayfalara bu şekilde cevap dönülür.
        if (string.IsNullOrEmpty(httpContextRequest.ContentType))
        {
            var redirectResult = new RedirectResult("/Account/Login" + returnUrl);
            context.Result = redirectResult;
            return;
        }
        var responseModel = GetSessionRespose();
        //Link üzerinden bir sayfaya gidilmek istediğinde aşağıda ki metod kullanılır.
        if (returnUrl.Contains("CheckCurrentUserSession"))
        {
            var obj = new { Data = string.Empty };
            returnUrl = string.Concat("?ReturnUrl=", httpContextRequest.GetPostData(obj)?.Data);
        }
        responseModel.ReturnUrl += returnUrl;
        context.Result = new JsonActionResult(responseModel, HttpStatusCode.OK.ToInt32());
    }

    private async Task<bool> IsUserAuthenticated(HttpContext httpContext)
    {
        var username = UserHelper.GetUserName(httpContext);
        var um = httpContext.RequestServices.GetService<IUserManager>();
        var userInfo = await um.GetByUserName(username);
        if (!userInfo.IsActive || userInfo.IsDeleted)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return false;
        }
        return UserHelper.IsUserAuthenticated(httpContext);
    }

    /// <summary>
    /// Only Axios requests are allowed
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private bool CheckIfRequestIsValid(HttpRequest httpContextRequest)
    {
        if (httpContextRequest.ContentType == KeyValues.JsonContentType)
            return httpContextRequest.Headers.Any(f => f.Key == "x-httprequest-type" && f.Value == "Axios");
        return true;
    }

    private HttpResponseModel<ErrorModel> GetSessionRespose()
    {
        var errorModel = new ErrorModel
        {
            StackTrace = string.Empty,
            ErrorType = ErrorType.Session,
        };
        return HttpResponseModel<ErrorModel>.Fail(errorModel, "Oturumunuz sona ermiştir.<br/>Giriş sayfasına yönlendiriliyorsunuz..!!", "/Account/Login");
    }

    private static bool IsAllowedAnonymous(AuthorizationFilterContext context)
    {
        return context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
    }

}
