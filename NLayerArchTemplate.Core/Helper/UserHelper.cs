using Microsoft.AspNetCore.Http;
using NLayerArchTemplate.Core.ConstantKeys;
using System.Security.Claims;

namespace NLayerArchTemplate.Core.Helper;
public static class UserHelper
{
    public static string GetUserName(HttpContext httpContext) => httpContext.User.Claims.First(f => f.Type == ClaimTypes.NameIdentifier)?.Value;

    public static string GetUserFullName(HttpContext httpContext) => httpContext.User.Claims.First(f => f.Type == KeyValues.ClaimTypeUserFullName)?.Value;

    public static bool IsUserAuthenticated(HttpContext httpContext) => httpContext.User.Identity.IsAuthenticated;

    public static string GetUserId(HttpContext httpContext) => httpContext.User.Claims.First(f => f.Type == KeyValues.ClaimTypeId)?.Value;

}
