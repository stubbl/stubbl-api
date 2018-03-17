using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Stubbl.Api
{
    public static class JsonConstants
    {
        static JsonConstants()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
            JsonSerializerSettings.Converters.Add(new StringEnumConverter());
        }

        public static JsonSerializerSettings JsonSerializerSettings { get; }
    }
}