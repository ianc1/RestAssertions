namespace RestAssertions.Utilities
{
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    internal static class JsonUtils
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

        public static string Normalize(string json)
        {
            return Serialize(ToDictionary(JToken.Parse(json)));
        }

        public static JToken CreateJToken(string json)
        {
            return JToken.Parse(json);
        }

        private static object ToDictionary(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.Children<JProperty>()
                        .ToDictionary(prop => prop.Name, prop => ToDictionary(prop.Value));

                case JTokenType.Array:
                    return token.Select(ToDictionary).ToList();

                default:
                    return ((JValue)token).Value;
            }
        }
    }
}
