using System.Text.Json;

namespace NLayerArchTemplate.Core.Extensions;

public static class ObjectExtensions
{
    public static string ToJSON(this object obj, JsonSerializerOptions jsonSerializerOptions = default)
    {
        return JsonSerializer.Serialize(obj, jsonSerializerOptions);
    }
}
