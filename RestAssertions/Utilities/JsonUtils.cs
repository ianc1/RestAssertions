namespace RestAssertions.Utilities
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class JsonUtils
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };

        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(
                value,
                Formatting.Indented,
                Settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}
