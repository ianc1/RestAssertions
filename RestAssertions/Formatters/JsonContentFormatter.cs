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
            var jToken = JTokenUtils.CreateJToken(json);

            Sort(jToken);

            string prettyJson = JsonUtils.Serialize(jToken);

            if (!includeLineNumbers)
            {
                return prettyJson;
            }

            return string.Join(NewLine, prettyJson.Split(NewLine).Select((line, index) => $"{index + 1,6}:  {line}"));
        }

        private static void Sort(JToken jToken)
        {
            if (jToken is JObject)
            {
                Sort((JObject)jToken);
            }

            if (jToken is JArray)
            {
                Sort((JArray)jToken);
            }
        }

        private static void Sort(JObject jObject)
        {
            var props = jObject.Properties().ToList();

            foreach (var prop in props)
            {
                prop.Remove();
            }

            foreach (var prop in props.OrderBy(p => p.Name))
            {
                jObject.Add(prop);
                Sort(prop.Value);
            }
        }

        private static void Sort(JArray jArray)
        {
            var iCount = jArray.Count;

            for (var iIterator = 0; iIterator < iCount; iIterator++)
            {
                Sort(jArray[iIterator]);
            }
        }
    }
}
