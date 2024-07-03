using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.Core.ConstantMessages;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.WebUI.Configuration.ActionResults;
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
        if (IsAllowedAnonymous(context) || await IsUserAuthenticated(httpContext)) return;
        var queryString = httpContextRequest.QueryString.ToString();
        var path = httpContextRequest.Path.Value;
        var returnUrl = string.Concat("?ReturnUrl=", path, queryString);
        //refresh yapılan sayfalara bu şekilde cevap dönülür.
        if (string.IsNullOrEmpty(httpContextRequest.ContentType))
        {
            var redirectResult = new RedirectResult(AccountUrlKeys.Login + returnUrl);
            context.Result = redirectResult;
            return;
        }
        var responseModel = GetUnAuthorizationRespose();
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

    private HttpResponseModel<ErrorModel> GetUnAuthorizationRespose()
    {
        var errorModel = new ErrorModel
        {
            StackTrace = string.Empty,
            ErrorType = ErrorType.Authorization,
        };
        return HttpResponseModel<ErrorModel>.Fail(errorModel, ErrorMessages.LogOut, AccountUrlKeys.Login);
    }

    private static bool IsAllowedAnonymous(AuthorizationFilterContext context)
    {
        return context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
    }
}
