//using NLayerArchTemplate.Core.ConstantKeys;
//using NLayerArchTemplate.Core.Enums;
//using NLayerArchTemplate.Core.Extensions;
//using NLayerArchTemplate.Core.Models;
//using System.Net;

//namespace NLayerArchTemplate.WebUI.Configuration.Middlewares;
////.Net Core API projelerinde global error handler middleware kullanmak daha doğru olur.
//public class ErrorHandlerMiddleware
//{
//    private readonly RequestDelegate _next;
//    public ErrorHandlerMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception ex)
//        {
//            await HandleExceptionMessageAsync(context, ex);
//        }
//    }

//    private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
//    {
//        context.Response.ContentType = KeyValues.JsonContentType;
//        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        var errorModel = new ErrorModel
//        {
//            Message = exception.GetMessage(),
//            StackTrace = exception.GetStackTrace(),
//            ErrorType = ErrorType.Default
//        };
//        var response = HttpResponseModel<ErrorModel>.Fail(errorModel);
//        return context.Response.WriteAsync(response.ToJSON());
//    }
//}
