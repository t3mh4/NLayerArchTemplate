using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayerArchTemplate.Core;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.WebUI.Configuration.ActionResults;
using NLayerArchTemplate.WebUI.Configuration.Extensions;
using NLayerArchTemplate.WebUI.Helpers;
using System.Diagnostics;
using System.Net;

namespace NLayerArchTemplate.WebUI.Configuration.Filters;
public sealed class DefaultAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var httpContextRequest = httpContext.Request;
        if (IsAllowedAnonymous(context) || IsUserAuthenticated(httpContext)) return;
        //client side'dan gelen http requestler axios ile yapılıp yapılmadığı kontrol ediliyor.
        if (!CheckIfRequestIsValid(httpContextRequest))
        {
            var statusCode = HttpStatusCode.BadRequest.ToInt32();
            var resModel = HttpResponseModel.Fail("Hatalı İşlem..!!");
            context.Result = new JsonActionResult(resModel, statusCode);
            return;
        }
        var queryString = httpContextRequest.QueryString.ToString();
        var path = httpContextRequest.Path.Value;
        var returnUrl = string.Concat("?ReturnUrl=", path, queryString);
        var xRequest = httpContextRequest.Headers["X-Requested-With"].ToString();
        if (string.IsNullOrWhiteSpace(xRequest))
        {
            var redirectResult = new RedirectResult("/Account/Login" + returnUrl);
            context.Result = redirectResult;
            return;
        }
        context.Result = new JsonActionResult(GetSessionRespose(), HttpStatusCode.BadRequest.ToInt32());
    }

    /*
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var httpContextRequest = httpContext.Request;
            //client side'dan gelen http requestler axios ile yapılıp yapılmadığı kontrol ediliyor.
            if (!CheckIfRequestIsValid(httpContextRequest))
            {
                var statusCode = HttpStatusCode.BadRequest.ToInt32();
                var resModel = HttpResponseModel.Fail("Hatalı İşlem..!!");
                context.Result = new JsonActionResult(resModel, statusCode);
                return;
            }
            if (IsAllowedAnonymous(context) || IsUserAuthenticated(httpContext)) return;
            var queryString = httpContextRequest.QueryString.ToString();
            var path = httpContextRequest.Path.Value;
            var returnUrl = string.Concat("?ReturnUrl=", path, queryString);
            //refresh yapılan sayfalara bu şekilde cevap dönülür.
            if (string.IsNullOrEmpty(httpContextRequest.ContentType))
            {
                var xRequest = httpContextRequest.Headers["X-Requested-With"].ToString();
                if (string.IsNullOrWhiteSpace(xRequest))
                {
                    var redirectResult = new RedirectResult("/Account/Login" + returnUrl);
                    context.Result = redirectResult;
                }
                else
                {
                    context.Result = new JsonActionResult(GetSessionRespose(), HttpStatusCode.BadRequest.ToInt32());
                }
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
            context.Result = new JsonActionResult(responseModel);
        }
        */
    private bool IsUserAuthenticated(HttpContext httpContext)
    {
        return UserHelper.IsUserAuthenticated(httpContext);
    }

    /// <summary>
    /// Only Axios requests are allowed
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private bool CheckIfRequestIsValid(HttpRequest httpContextRequest)
    {
        var type = httpContextRequest.Headers["X-Requested-With"].ToString();
        if (!string.IsNullOrWhiteSpace(type))
            return httpContextRequest.Headers.Any(f => f.Key == "x-httprequest-type" && f.Value == "Axios");
        return true;
    }

    private HttpResponseModel<ErrorModel> GetSessionRespose(string returnUrl= "/Account/Login")
    {
        var errorModel = new ErrorModel
        {
            StackTrace = string.Empty,
            ErrorType = ErrorType.Session,
        };
        return HttpResponseModel<ErrorModel>.Fail(errorModel, "Oturumunuz sona ermiştir.<br/>Giriş sayfasına yönlendiriliyorsunuz..!!", returnUrl);
    }

    private static bool IsAllowedAnonymous(AuthorizationFilterContext context)
    {
        return context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
    }

}
