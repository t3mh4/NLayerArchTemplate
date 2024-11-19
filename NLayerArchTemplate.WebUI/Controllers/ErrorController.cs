using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.Core.Exceptions;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.WebUI.Configuration.ActionResults;
using Serilog;
using System.Net;

namespace NLayerArchTemplate.WebUI.Controllers;

[AllowAnonymous]
public class ErrorController : Controller
{
    private ErrorModel _errorModel = new ErrorModel();

    [Route("Error/{code}")]
    public IActionResult Index(int code, [FromQuery] string path, [FromQuery] string queryString)
    {
        HttpContext.Response.StatusCode = code;
        var requestPath = path + queryString;
        SaveLog(requestPath);
        _errorModel.Message = $"URL = {requestPath}";
        return View(_errorModel);
    }

    [Route("Error/Handle")]
    public IActionResult Handle()
    {
        var code = HttpStatusCode.InternalServerError.ToInt32();
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var exception = feature.Error;
        if (HttpContext.Request.Headers.XRequestedWith.Count == 0)
        {
            var originalPath = feature.Path;
            var originalQueryString = HttpContext.Request.QueryString.Value;
            var redirectUrl = $"/Error/{HttpContext.Response.StatusCode}?path={originalPath}&queryString={originalQueryString}";
            SaveLog(originalPath, exception);
            return new RedirectResult(redirectUrl);
        }
        switch (exception)
        {
            case ValidationException:
                _errorModel = ErrorModel.Create((exception as ValidationException)?.GetMessage(), String.Empty, ErrorType.Validation);//GetValidationExceptionModel(exception as ValidationException);
                code = HttpStatusCode.BadRequest.ToInt32();
                break;
            case DataNotFoundException:
                _errorModel = ErrorModel.Create(exception.GetMessage(), String.Empty, ErrorType.Default);//GetDataNotFoundExceptionModel(exception as DataNotFoundException);
                code = HttpStatusCode.BadRequest.ToInt32();
                break;
            case CustomException:
                _errorModel = ErrorModel.Create(exception.GetMessage(), String.Empty, ErrorType.Default);//GetCustomExceptionModel(exception as CustomException);
                code = HttpStatusCode.BadRequest.ToInt32();
                break;
            default:
                _errorModel = ErrorModel.Create(exception.GetMessage(), exception.GetStackTrace(), ErrorType.Default);//GetExceptionModel(exception);
                break;
        }
        var hostingEnv = HttpContext.RequestServices.GetService<IWebHostEnvironment>();
        if (hostingEnv.IsProduction())
        {
            if (code == HttpStatusCode.InternalServerError.ToInt32())
            {
                //NOT:Aşağıdaki kodu sistemin aynı logdan 2 tane kaydetmemesi için kapattım!!
                //SaveLog(exception, feature.Path, GetHttpMethodType(feature.Endpoint.Metadata));
                _errorModel.Message = "Beklenmedik bir hata ile karşılaşıldı..!!";
            }
            _errorModel.StackTrace = string.Empty;
        }
        var response = HttpResponseModel<ErrorModel>.Fail(_errorModel);
        return new JsonActionResult(response, code);
    }

    private string GetHttpMethodType(EndpointMetadataCollection metadataCollection)
    {
        var httpMethodMetadata = metadataCollection.GetMetadata<HttpMethodMetadata>();
        if (httpMethodMetadata != null)
        {
            return httpMethodMetadata.HttpMethods.First();
        }
        return string.Empty;
    }
    //private void SaveLog(Exception exception, string requestPath, string requestType)
    //{
    //    var userFullName = HttpContext.User.Identity.IsAuthenticated ? HttpContext.User.Claims.First(f => f.Type == KeyValues.ClaimTypeUserFullName)?.Value : "Anonymous";
    //    LogContext.PushProperty(KeyValues.ClaimTypeUserFullName, userFullName); //Push user in LogContext;  
    //    LogContext.PushProperty("RequestPath", requestPath); //Push user in LogContext; 
    //    LogContext.PushProperty("RequestType", requestType); //Push user in LogContext; 
    //    LogContext.PushProperty("StatusCode", HttpContext.Response.StatusCode); //Push user in LogContext; 
    //    Log.Logger.Error(exception, "Beklenmedik bir hata ile karşılaşıldı..!!");
    //}
    private void SaveLog(string requestPath, Exception ex = default)
    {
        if (HttpContext.Response.StatusCode >= 400)
        {
            var userFullName = HttpContext.User.Identity.IsAuthenticated ? HttpContext.User.Claims.First(f => f.Type == KeyValues.ClaimTypeUserFullName)?.Value : "Anonymous";
            Log.Logger.Error(ex, "{RequestType} Methodu ile {RequestPath} adresine {UserFullName} kullanıcı adı ile gönderilen istek için HTTP {StatusCode} hatası oluştu",
                HttpContext.Request.Method,
                requestPath,
                userFullName,
                HttpContext.Response.StatusCode);
        }
    }
}
