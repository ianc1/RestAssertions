namespace RestAssertions.Utilities
{
    using System;
    using System.Text.RegularExpressions;

    using Newtonsoft.Json.Linq;

    public class JsonTokenUtils
    {
        public static JToken CreateJToken(string json)
        {
            return JToken.Parse(json);
        }

        public static JToken GetProperty(string json, string propertyPath)
        {
            var jsonTokenUtils = new JsonTokenUtils();

            return jsonTokenUtils.GetChildJToken(json, propertyPath);
        }

        private JToken GetChildJToken(string json, string childPath)
        {
            var jsonToken = CreateJToken(json);

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
                    (path, jsonToken) = GetJToken(path, node, jsonToken);
                }

                if (arrayIndex >= 0)
                {
                    (path, jsonToken) = GetArrayJToken(path, arrayIndex, jsonToken);
                }
            }

            return jsonToken;
        }

        private void ExpectJTokenType(string path, JTokenType expectedType, JToken jsonToken)
        {
            if (jsonToken.Type != expectedType)
            {
                ThrowException(path, $"was not of type '{expectedType}'");
            }
        }

        private void ThrowException(string path, string error)
        {
            var tokenPathMessage = path == string.Empty ? "JSON root token" : $"JSON token '{path}'";

            throw new Exception($"{tokenPathMessage} {error}");
        }

        private (string path, JToken jToken) GetJToken(string path, string node, JToken jsonToken)
        {
            ExpectJTokenType(path, JTokenType.Object, jsonToken);

            if (!((JObject)jsonToken).TryGetValue(node, out var jsonTokenChild))
            {
                ThrowException(path, $"did not have property '{node}'");
            }

            path += path == string.Empty ? node : $".{node}";

            return (path, jsonTokenChild);
        }

        private (string path, JToken jToken) GetArrayJToken(string path, int arrayIndex, JToken jsonToken)
        {
            ExpectJTokenType(path, JTokenType.Array, jsonToken);

            path += $"[{arrayIndex}]";

            if (arrayIndex >= ((JArray)jsonToken).Count)
            {
                throw new Exception($"Expected JSON array index {arrayIndex}");
            }

            return (path, ((JArray)jsonToken)[arrayIndex]);
        }
    }
}
