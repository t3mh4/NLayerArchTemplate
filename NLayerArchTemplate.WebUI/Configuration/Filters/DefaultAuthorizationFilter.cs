using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.Core.ConstantMessages;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Core.Helper;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.WebUI.Configuration.ActionResults;
using System.Net;

namespace NLayerArchTemplate.WebUI.Configuration.Filters;
public sealed class DefaultAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var httpContextRequest = httpContext.Request;
        if (IsAllowedAnonymous(context) || UserHelper.IsUserAuthenticated(httpContext)) return;
        //client side'dan gelen http requestler axios ile yapılıp yapılmadığı kontrol ediliyor.
        var queryString = httpContextRequest.QueryString.ToString();
        var path = httpContextRequest.Path.Value;
        var returnUrl = string.Concat(AccountUrlKeys.Login, "?ReturnUrl=", path, queryString);
        if (httpContextRequest.Headers.XRequestedWith.Count == 0)
        {
            var redirectResult = new RedirectResult(returnUrl);
            context.Result = redirectResult;
            return;
        }
        context.Result = new JsonActionResult(GetUnAuthorizationRespose(returnUrl), HttpStatusCode.Unauthorized.ToInt32());
    }

    private HttpResponseModel<ErrorModel> GetUnAuthorizationRespose(string returnUrl = AccountUrlKeys.Login)
    {
        var errorModel = new ErrorModel
        {
            StackTrace = string.Empty,
            ErrorType = ErrorType.Authorization,
        };
        return HttpResponseModel<ErrorModel>.Fail(errorModel, ErrorMessages.LogOut, returnUrl);
    }

    private static bool IsAllowedAnonymous(AuthorizationFilterContext context)
    {
        return context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
    }

}
