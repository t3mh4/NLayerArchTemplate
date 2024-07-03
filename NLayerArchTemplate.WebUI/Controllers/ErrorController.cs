using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Core.ConstantKeys;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.WebUI.Configuration.ActionResults;
using Serilog;
using Serilog.Context;
using System.Net;

namespace NLayerArchTemplate.WebUI.Controllers;

[AllowAnonymous]
public class ErrorController : Controller
{
    private ErrorModel _errorModel = new ErrorModel();

    [Route("Error/{code}")]
    public IActionResult Index(int code, [FromQuery] string originalPath, [FromQuery] string originalQueryString)
    {
        HttpContext.Response.StatusCode = code;
        var requestPath = originalPath + originalQueryString;
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
        if (!HttpContext.Request.Headers.Any(a => a.Key == KeyValues.XRequestedWith))
        {
            var originalPath = feature.Path;
            var originalQueryString = HttpContext.Request.QueryString.Value;
            var redirectUrl = $"/Error/{HttpContext.Response.StatusCode}?originalPath={originalPath}&originalQueryString={originalQueryString}";
            SaveLog(originalPath);
            return new RedirectResult(redirectUrl);
        }
        switch (exception)
        {
            case ValidationException:
                _errorModel = GetValidationExceptionModel(exception as ValidationException);
                code = HttpStatusCode.BadRequest.ToInt32();
                break;
            default:
                _errorModel = GetExceptionModel(exception);
                break;
        }
        if (code == HttpStatusCode.InternalServerError.ToInt32())
        {
            SaveLog(exception, feature.Path, GetHttpMethodType(feature.Endpoint.Metadata));
            _errorModel.Message = "Beklenmedik bir hata ile karşılaşıldı..!!";
        }
        var response = HttpResponseModel<ErrorModel>.Fail(_errorModel);
        return new JsonActionResult(response, code);
    }

    private ErrorModel GetValidationExceptionModel(ValidationException exception)
    {
        return new ErrorModel
        {
            Message = exception.GetMessage(),
            StackTrace = String.Empty,
            ErrorType = ErrorType.Validation
        };
    }

    private ErrorModel GetExceptionModel(Exception exception)
    {
        return new ErrorModel
        {
            Message = exception.GetMessage(),
            StackTrace = exception.GetStackTrace(),
            ErrorType = ErrorType.Default
        };
    }

    private void SaveLog(Exception exception, string requestPath, string requestType)
    {
        var userFullName = HttpContext.User.Identity.IsAuthenticated ? HttpContext.User.Claims.First(f => f.Type == "UserFullName")?.Value : "Anonymous";
        LogContext.PushProperty("UserFullName", userFullName); //Push user in LogContext;  
        LogContext.PushProperty("RequestPath", requestPath); //Push user in LogContext; 
        LogContext.PushProperty("RequestType", requestType); //Push user in LogContext; 
        LogContext.PushProperty("StatusCode", HttpContext.Response.StatusCode); //Push user in LogContext; 
        Log.Logger.Error(exception, "Beklenmedik bir hata ile karşılaşıldı..!!");
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

    private void SaveLog(string requestPath)
    {
        if (HttpContext.Response.StatusCode >= 400)
        {
            var userFullName = HttpContext.User.Identity.IsAuthenticated ? HttpContext.User.Claims.First(f => f.Type == "UserFullName")?.Value : "Anonymous";
            Log.Logger.Error("{RequestType} Methodu ile {RequestPath} adresine {UserFullName} kullanıcı adı ile gönderilen istek için HTTP {StatusCode} hatası oluştu",
                HttpContext.Request.Method,
                requestPath,
                userFullName,
                HttpContext.Response.StatusCode);
        }
    }
}
