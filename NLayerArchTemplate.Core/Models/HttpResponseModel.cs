using NLayerArchTemplate.Core.Abstracts;

namespace NLayerArchTemplate.Core.Models;

public class HttpResponseModel : AResponse
{
    public static HttpResponseModel Success()
    {
        return new HttpResponseModel
        {
            IsSuccess = true
        };
    }

    public static HttpResponseModel Success(string message)
    {
        return new HttpResponseModel
        {
            Message = message,
            IsSuccess = true
        };
    }

    public static HttpResponseModel Success(string message, string returnUrl)
    {
        return new HttpResponseModel
        {
            Message = message,
            IsSuccess = true,
            ReturnUrl = returnUrl
        };
    }

    public static HttpResponseModel Fail()
    {
        return new HttpResponseModel
        {
            IsSuccess = false,
        };
    }

    public static HttpResponseModel Fail(string message)
    {
        return new HttpResponseModel
        {
            IsSuccess = false,
            Message = message
        };
    }

    public static HttpResponseModel Fail(string message, string returnUrl)
    {
        return new HttpResponseModel
        {
            IsSuccess = false,
            Message = message,
            ReturnUrl = returnUrl
        };
    }
}