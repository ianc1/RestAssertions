namespace RestAssertions.Assertions
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Utilities;

    public static class HttpResponseAssertions
    {
        public static async Task ShouldBe(this HttpResponseMessage httpResponseMessage, HttpStatusCode expectedStatusCode, object expectedContent, string customMessage = null)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            HttpResponseCodeShouldEqual(expectedStatusCode, httpResponseMessage.StatusCode, content);

            var actualContent = JsonUtils.Deserialize<dynamic>(content);

            JsonContentShouldBeEqual(expectedContent, actualContent);
        }

        public static async Task ShouldBe(this HttpResponseMessage httpResponseMessage, HttpStatusCode expectedStatusCode)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            HttpResponseCodeShouldEqual(expectedStatusCode, httpResponseMessage.StatusCode, content);
        }

        private static void HttpResponseCodeShouldEqual(HttpStatusCode expectedCode, HttpStatusCode actualCode, string content)
        {
            if (expectedCode == actualCode)
            {
                return;
            }

            throw new RestAssertionException("HTTP response code", expectedCode, actualCode, $"HTTP response content was:\r\n{content}");
        }

        private static void JsonContentShouldBeEqual(object expectedContent, object actualContent)
        {
            var expectedJson = JsonUtils.Serialize(expectedContent);
            var actualJson = JsonUtils.Serialize(actualContent);

            var expectedJsonLines = expectedJson.Split("\r\n").Select((line, index) => $"{index + 1, 6}:  {line}\r\n").ToList();
            var actualJsonLines = actualJson.Split("\r\n").Select((line, index) => $"{index + 1, 6}:  {line}\r\n").ToList();

            for (var i = 0; i < actualJsonLines.Count; i++)
            {
                if (actualJsonLines[i] == expectedJsonLines[i])
                {
                    continue;
                }

                actualJsonLines[i] = '*' + actualJsonLines[i].Substring(1);

                if (expectedJsonLines.Count > i)
                {
                    expectedJsonLines[i] = '*' + expectedJsonLines[i].Substring(1);
                }

                throw new RestAssertionException("HTTP response content", string.Concat(expectedJsonLines), string.Concat(actualJsonLines), $"Difference: Line {i + 1}");
            }
        }
    }
}
