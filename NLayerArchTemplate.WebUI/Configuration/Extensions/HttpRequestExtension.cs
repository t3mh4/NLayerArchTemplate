using NLayerArchTemplate.Core.Extensions;

namespace NLayerArchTemplate.WebUI.Configuration.Extensions;

public static class HttpRequestExtension
{
    public static T GetPostData<T>(this HttpRequest request, T definition) where T : class
    {
        var postDataStr = GetPostData(request);
        var obj = postDataStr.ToAnonymousObject(definition);
        return obj;
    }

    private static string GetPostData(HttpRequest request)
    {
        var reader = new StreamReader(request.Body);
        var requestFromPost = reader.ReadToEndAsync().Result;
        return requestFromPost;
    }
}
