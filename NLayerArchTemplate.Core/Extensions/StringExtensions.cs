using System.Text.Json;

namespace NLayerArchTemplate.Core.Extensions;

public static class StringExtensions
{
    public static T ToObject<T>(this string value) where T : class
    {
        try
        {
            return JsonSerializer.Deserialize<T>(value);
        }
        catch
        {
            return default;
        }
    }

    public static T ToAnonymousObject<T>(this string value, T definition, JsonSerializerOptions options = default) where T : class
    {
        try
        {
            return JsonSerializer.Deserialize<T>(value, options);
            //return JsonConvert.DeserializeAnonymousType(value, definition);
        }
        catch
        {
            return default;
        }
    }
}
