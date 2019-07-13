namespace RestAssertions.Formatters
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using RestAssertions.Utilities;
    using static FormatUtils;

    internal static class JsonContentFormatter
    {
        public static string Format(string json, bool includeLineNumbers = true)
        {
            var jsonToken = JsonTokenUtils.CreateJToken(json);

            Sort(jsonToken);

            string prettyJson = JsonUtils.Serialize(jsonToken);

            if (!includeLineNumbers)
            {
                return prettyJson;
            }

            return string.Join(NewLine, prettyJson.Split(NewLine).Select((line, index) => $"{index + 1,6}:  {line}"));
        }

        private static void Sort(JToken jsonToken)
        {
            if (jsonToken is JObject)
            {
                Sort((JObject)jsonToken);
            }

            if (jsonToken is JArray)
            {
                Sort((JArray)jsonToken);
            }
        }

        private static void Sort(JObject jsonObject)
        {
            var props = jsonObject.Properties().ToList();

            foreach (var prop in props)
            {
                prop.Remove();
            }

            foreach (var prop in props.OrderBy(p => p.Name))
            {
                jsonObject.Add(prop);
                Sort(prop.Value);
            }
        }

        private static void Sort(JArray jsonArray)
        {
            var jsonArrayCount = jsonArray.Count;

            for (var index = 0; index < jsonArrayCount; index++)
            {
                Sort(jsonArray[index]);
            }
        }
    }
}
