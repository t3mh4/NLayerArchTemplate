using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Business.Validators.User;
using NLayerArchTemplate.Core.ConstantMessages;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.Dtos.User;
using NLayerArchTemplate.WebUI.Configuration.ActionResults;
using NtierArchTemplate.Business.UserManager;
using NtierArchTemplate.Business.Validators;

namespace NLayerArchTemplate.WebUI.Controllers
{
    public class UserController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.PageHeader = "Kullanıcı Listesi";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var userManager = GetManager<IUserManager>();
            var userList = await userManager.GetUserList(ct);
            var response = HttpResponseModel<List<UserListItemDto>>.Success(userList);
            return new JsonActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CorE([FromBody] HttpRequestModel<int> httpRequest, CancellationToken ct)
        {
            Validator<UserCoreValidator>.Validate(httpRequest.Data);
            var userManager = GetManager<IUserManager>();
            var user = await userManager.GetByUserId(httpRequest.Data, ct);
            return PartialView(user);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] HttpSaveRequestModel<UserSaveDto> httpRequest, CancellationToken ct)
        {
            Validator<UserSaveValidator>.Validate(httpRequest.Data);
            var msg = httpRequest.Data.Id == 0 ? SuccessCrudMessages.Create : SuccessCrudMessages.Update;
            var userManager = GetManager<IUserManager>();
            var result = await userManager.Save(httpRequest.Data, httpRequest.ModifiedProperties, ct);
            var response = HttpResponseModel<int>.Success(result, msg);
            return new JsonActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] HttpRequestModel<int> httpRequest, CancellationToken ct)
        {
            throw new Exception("hata var");
            Validator<UserDeleteValidator>.Validate(httpRequest.Data);
            var userManager = GetManager<IUserManager>();
            await userManager.Delete(httpRequest.Data, ct);
            var response = HttpResponseModel.Success(SuccessCrudMessages.Delete);
            return new JsonActionResult(response);
        }
    }
}
