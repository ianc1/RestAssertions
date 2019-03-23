namespace RestAssertions.Formatters
{
    using System.Linq;

    using Utilities;

    using static FormatUtils;

    public static class JsonContentFormatter
    {
        public static string Format(object jsonObject)
        {
            string prettyJson = JsonUtils.Serialize(jsonObject);

            return string.Join(NewLine, prettyJson.Split(NewLine).Select((line, index) => $"{index + 1, 6}:  {line}"));
        }

        public static string Format(string jsonString)
        {
            return Format(JsonUtils.Deserialize<dynamic>(jsonString));
        }
    }
}
