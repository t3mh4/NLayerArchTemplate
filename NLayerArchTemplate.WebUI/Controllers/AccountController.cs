using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.Core.ConstantMessages;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.Dtos.Login;
using NLayerArchTemplate.WebUI.Configuration.ActionResults;
using NLayerArchTemplate.Business.UserManager;
using NLayerArchTemplate.Business.Validators;
using System.Net;
using System.Security.Claims;

namespace NLayerArchTemplate.WebUI.Controllers;

public class AccountController : BaseController
{
    private readonly IServiceProvider _serviceProvider;

    public AccountController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login([FromQuery] string returnUrl)
    {
        if (HttpContext.User.Identity.IsAuthenticated)
            return RedirectToAction("", string.IsNullOrWhiteSpace(returnUrl) ? "Home" : returnUrl);
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] HttpRequestModel<LoginDto> request, CancellationToken cancellationToken)
    {
        Validator<SignInValidator>.Validate(request.Data, _serviceProvider);
        var userManager = GetManager<IUserManager>();
        var user = await userManager.CheckAuthorization(request.Data, cancellationToken);
        if (user == null)
        {
            return new JsonActionResult(HttpResponseModel.Fail(AccountMessages.CheckAuthorizationFail));
        }
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier,request.Data.Username),
            new(KeyValues.ClaimTypeUserFullName,$"{user.Name} {user.Surname}"),
            new(KeyValues.ClaimTypeEmail,user.Email),			
            new(KeyValues.ClaimTypeId,user.Id.ToString())
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        return new JsonActionResult(HttpResponseModel.Success(AccountMessages.LoginSuccess(user.UserFullName)));
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new JsonActionResult(HttpResponseModel.Success(AccountMessages.Logout, AccountUrlKeys.Login));
    }
}
