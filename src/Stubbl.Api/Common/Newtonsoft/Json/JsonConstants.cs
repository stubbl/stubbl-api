namespace Newtonsoft.Json
{
    using Converters;
    using Serialization;

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