namespace RestAssertions.Utilities
{
    using System;
    using System.Text.RegularExpressions;

    using Newtonsoft.Json.Linq;

    public class JTokenUtils
    {
        public static JToken CreateJToken(string json)
        {
            return JToken.Parse(json);
        }

        public static JToken GetProperty(string json, string propertyPath)
        {
            var jTokenUtils = new JTokenUtils();

            return jTokenUtils.GetChildJToken(json, propertyPath);
        }

        private JToken GetChildJToken(string json, string childPath)
        {
            var jToken = CreateJToken(json);

            var regex = new Regex("(?<property>.*?)\\[(?<arrayIndex>[0-9]+)\\]");

            var path = string.Empty;

            foreach (var node in childPath.Split("."))
            {
                var property = node;
                var arrayIndex = -1;
                var match = regex.Match(node);

                if (match.Success)
                {
                    property = match.Groups[1].Value;
                    arrayIndex = int.Parse(match.Groups[2].Value);
                }

                if (!string.IsNullOrEmpty(property))
                {
                    (path, jToken) = GetJToken(path, node, jToken);
                }

                if (arrayIndex >= 0)
                {
                    (path, jToken) = GetArrayJToken(path, arrayIndex, jToken);
                }
            }

            return jToken;
        }

        private void ExpectJTokenType(string path, JTokenType expectedType, JToken jToken)
        {
            if (jToken.Type != expectedType)
            {
                ThrowException(path, $"was not of type '{expectedType}'");
            }
        }

        private void ThrowException(string path, string error)
        {
            var tokenPathMessage = path == string.Empty ? "JSON root token" : $"JSON token '{path}'";

            throw new Exception($"{tokenPathMessage} {error}");
        }

        private (string path, JToken jToken) GetJToken(string path, string node, JToken jToken)
        {
            ExpectJTokenType(path, JTokenType.Object, jToken);

            if (!((JObject)jToken).TryGetValue(node, out var jTokenChild))
            {
                ThrowException(path, $"did not have property '{node}'");
            }

            path += path == string.Empty ? node : $".{node}";

            return (path, jTokenChild);
        }

        private (string path, JToken jToken) GetArrayJToken(string path, int arrayIndex, JToken jToken)
        {
            ExpectJTokenType(path, JTokenType.Array, jToken);

            path += $"[{arrayIndex}]";

            if (arrayIndex >= ((JArray)jToken).Count)
            {
                throw new Exception($"Expected JSON array index {arrayIndex}");
            }

            return (path, ((JArray)jToken)[arrayIndex]);
        }
    }
}
