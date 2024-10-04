using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Business.Validators.User;
using NLayerArchTemplate.Core.ConstantMessages;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.Dtos.User;
using NLayerArchTemplate.WebUI.Configuration.ActionResults;
using NLayerArchTemplate.Business.UserManager;
using NLayerArchTemplate.Business.Validators;
using NLayerArchTemplate.Core.Helper;

namespace NLayerArchTemplate.WebUI.Controllers;

public class UserController : BaseController
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserManager _userManager;

    public UserController(IServiceProvider serviceProvider, IUserManager userManager)
    {
        _serviceProvider = serviceProvider;
        _userManager = userManager;
    }

    [Route("User")]
    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.PageHeader = "Kullanıcı Listesi";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var userList = await _userManager.GetUserList(ct);
        var response = HttpResponseModel<List<UserListItemDto>>.Success(userList);
        return new JsonActionResult(response);
    }

    [HttpPost]
    public async Task<IActionResult> CorE([FromBody] HttpRequestModel<int> httpRequest, CancellationToken ct)
    {
        Validator<UserIdValidator>.Validate(httpRequest.Data, _serviceProvider);
        var user = await _userManager.GetByUserId(httpRequest.Data, ct);
        return PartialView(user);
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] HttpSaveRequestModel<UserSaveDto> httpRequest, CancellationToken ct)
    {
        Validator<UserSaveValidator>.Validate(httpRequest.Data, _serviceProvider);
        var msg = httpRequest.Data.Id == 0 ? SuccessCrudMessages.Create : SuccessCrudMessages.Update;
        var result = await _userManager.Save(httpRequest.Data, httpRequest.ModifiedProperties, ct);
        var response = HttpResponseModel<int>.Success(result, msg);
        return new JsonActionResult(response);
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] HttpRequestModel<int> httpRequest, CancellationToken ct)
    {
        HttpResponseModel response;
        Validator<UserDeleteValidator>.Validate(httpRequest.Data, _serviceProvider);
        if (httpRequest.Data.ToString() == UserHelper.GetUserId(HttpContext))
        {
            response = HttpResponseModel.Fail("Şuan bu kullanıcı ile giriş yaptığınız için silinemez..!!");
            return new JsonActionResult(response);
        }
        await _userManager.Delete(httpRequest.Data, ct);
        response = HttpResponseModel.Success(SuccessCrudMessages.Delete);
        return new JsonActionResult(response);
    }
}
