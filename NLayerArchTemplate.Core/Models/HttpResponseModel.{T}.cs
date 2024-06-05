using NLayerArchTemplate.Core.Abstracts;

namespace NLayerArchTemplate.Core.Models;

public class HttpResponseModel<T> : AResponse
{
    public T Data { get; set; } = default;

    public static HttpResponseModel<T> Success(T data)
    {
        return new HttpResponseModel<T>
        {
            Data = data,
            IsSuccess = true
        };
    }
    public static HttpResponseModel<T> Success(T data, string message)
    {
        return new HttpResponseModel<T>
        {
            Data = data,
            IsSuccess = true,
            Message = message
        };
    }
    public static HttpResponseModel<T> Success(T data, string message, string returnUrl)
    {
        return new HttpResponseModel<T>
        {
            Data = data,
            IsSuccess = true,
            Message = message,
            ReturnUrl = returnUrl
        };
    }
    public static HttpResponseModel<T> Fail(T data)
    {
        return new HttpResponseModel<T>
        {
            Data = data,
            IsSuccess = false,
        };
    }
    public static HttpResponseModel<T> Fail(T data, string message)
    {
        return new HttpResponseModel<T>
        {
            Data = data,
            IsSuccess = false,
            Message = message
        };
    }
    public static HttpResponseModel<T> Fail(T data, string message, string returnUrl)
    {
        return new HttpResponseModel<T>
        {
            Data = data,
            IsSuccess = false,
            Message = message,
            ReturnUrl = returnUrl
        };
    }
}