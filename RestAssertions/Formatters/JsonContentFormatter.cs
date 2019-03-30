namespace RestAssertions.Formatters
{
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using Utilities;

    using static FormatUtils;

    public static class JsonContentFormatter
    {
        public static string Format(object jsonObject)
        {
            return Format(JToken.FromObject(jsonObject));
        }

        public static string Format(string jsonString)
        {
            return Format(JToken.Parse(jsonString));
        }

        private static string Format(JToken jToken)
        {
            Sort(jToken);

            string prettyJson = JsonUtils.Serialize(jToken);

            return string.Join(NewLine, prettyJson.Split(NewLine).Select((line, index) => $"{index + 1, 6}:  {line}"));
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
