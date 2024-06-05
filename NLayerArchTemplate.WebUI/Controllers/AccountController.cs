﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.Dtos.Login;
using NLayerArchTemplate.WebUI.Configuration.ActionResults;
using NtierArchTemplate.Business.UserManager;
using NtierArchTemplate.Business.Validators;
using System.Net;
using System.Security.Claims;

namespace NLayerArchTemplate.WebUI.Controllers;

public class AccountController : BaseController
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] HttpRequestModel<LoginDto> request, CancellationToken cancellationToken)
    {
        Validator<SignInValidator>.Validate(request.Data);
        var userManager = GetManager<IUserManager>();
        var user = await userManager.CheckAuthorization(request.Data, cancellationToken);
        if (user == null)
        {
            return new JsonActionResult(HttpResponseModel.Fail("Kullanıcı adı veya şifre hatalı"), HttpStatusCode.InternalServerError.ToInt32());
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,request.Data.Username),
            new Claim(KeyValues.ClaimTypeUserFullName,$"{user.Name} {user.Surname}"),
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        return new JsonActionResult(HttpResponseModel.Success($"Hoş geldiniz {user.Name} {user.Surname}.<br/>Anasayfaya yönlendiriliyorsunuz."));
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new JsonActionResult(HttpResponseModel.Success("Giriş Sayfasına Yönlendiriliyorsunuz..!!", "/Account/Login"));
    }

    //[HttpPost]
    //public async Task<IActionResult> CheckCurrentUserSession(CancellationToken ct = default)
    //{
    //    //buraya gelmenden önce CustomAuthorization'a uğradığı için burada kontrol yapmaya gerek yok.
    //    await Task.CompletedTask;
    //    return new JsonActionResult(HttpResponseModel<bool>.Success(true));
    //}
}
