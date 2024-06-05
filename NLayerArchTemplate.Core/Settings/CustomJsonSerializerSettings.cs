using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NLayerArchTemplate.Core.Settings
{
    public class CustomJsonSerializerSettings : JsonSerializerSettings
    {
        public CustomJsonSerializerSettings()
        {
            // Json.NET will ignore objects in reference loops and not serialize them.
            // The first time an object is encountered it will be serialized as usual but
            // if the object is encountered as a child object of itself the serializer will skip serializing it.
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            DateFormatString = "dd.MM.yyyy HH:mm:ss.FFFFFFFK";
            Formatting = Formatting.Indented;
            //all enums of the target objects are serialized in a string form.
            //Converters = new List<JsonConverter> { new StringEnumConverter(new CamelCaseNamingStrategy()) };
            // Use the default property (Pascal) casing
            ContractResolver = new DefaultContractResolver();
        }
    }
}
