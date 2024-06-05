using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.Core.Extensions;
using NLayerArchTemplate.Core.Models;
using NLayerArchTemplate.WebUI.Configuration.ActionResults;
using Serilog;
using Serilog.Context;
using System.Diagnostics;
using System.Net;

namespace NLayerArchTemplate.WebUI.Controllers;

[AllowAnonymous]
public class ErrorController : Controller
{
    private ErrorModel _errorModel = new ErrorModel();

    [HttpGet]
    public IActionResult Index(int code)
    {
        var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        var originalPathAndQuery = string.Join(
                 statusCodeReExecuteFeature.OriginalPathBase,
                 statusCodeReExecuteFeature.OriginalPath,
                 statusCodeReExecuteFeature.OriginalQueryString);
        _errorModel.Message = $"URL = {originalPathAndQuery}";
        HttpContext.Response.StatusCode = code;
        return View(_errorModel);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public IActionResult Handle()
    {
        var code = HttpStatusCode.InternalServerError.ToInt32();
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();        
        var exception = feature.Error;
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
            SaveLog(exception, feature.Path, GetHttpMethodType(feature.Endpoint.Metadata));
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

    private void SaveLog(Exception exception,string requestPath,string requestType)
    {
        var userFullName = HttpContext.User.Identity.IsAuthenticated ? HttpContext.User.Claims.First(f => f.Type == "UserFullName")?.Value : "Guest";
        LogContext.PushProperty("UserFullName", userFullName); //Push user in LogContext;  
        LogContext.PushProperty("RequestPath", requestPath); //Push user in LogContext; 
        LogContext.PushProperty("RequestType", requestType); //Push user in LogContext; 
        Log.Logger.Error(exception, "An unhandled exception has occurred");
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
}
