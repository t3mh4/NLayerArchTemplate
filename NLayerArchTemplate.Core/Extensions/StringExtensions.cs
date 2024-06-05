using Newtonsoft.Json;

namespace NLayerArchTemplate.Core.Extensions;

public static class StringExtensions
{
    public static T ToObject<T>(this string value) where T : class
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        catch
        {
            return default;
        }
    }

    public static T ToAnonymousObject<T>(this string value, T definition) where T : class
    {
        try
        {
            return JsonConvert.DeserializeAnonymousType(value, definition);
        }
        catch
        {
            return default;
        }
    }
}
