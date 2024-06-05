using Newtonsoft.Json;
using NLayerArchTemplate.Core.Settings;

namespace NLayerArchTemplate.Core.Extensions;

public static class ObjectExtensions
{
    public static string ToJSON(this object obj)
    {
        return JsonConvert.SerializeObject(obj, new CustomJsonSerializerSettings());
    }
}
