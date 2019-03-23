namespace RestAssertions.Assertions
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mime;
    using System.Threading.Tasks;

    using Formatters;
    using Utilities;
    using static Formatters.FormatUtils;

    public static class HttpResponseAssertions
    {
        public static async Task ShouldBe(this HttpResponseMessage httpResponseMessage, HttpStatusCode expectedStatusCode, object expectedContent, string customMessage = null)
        {
            var contentType = httpResponseMessage.Content.Headers.ContentType;
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            HttpResponseCodeShouldEqual(expectedStatusCode, httpResponseMessage.StatusCode, contentType, content);

            var actualContent = JsonUtils.Deserialize<dynamic>(content);

            JsonContentShouldBeEqual(expectedContent, actualContent);
        }

        public static async Task ShouldBe(this HttpResponseMessage httpResponseMessage, HttpStatusCode expectedStatusCode)
        {
            var contentType = httpResponseMessage.Content.Headers.ContentType;
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            HttpResponseCodeShouldEqual(expectedStatusCode, httpResponseMessage.StatusCode, contentType, content);
        }

        private static void HttpResponseCodeShouldEqual(HttpStatusCode expectedCode, HttpStatusCode actualCode, MediaTypeHeaderValue contentType, string content)
        {
            if (expectedCode == actualCode)
            {
                return;
            }

            var formattedContent = contentType.MediaType == MediaTypeNames.Application.Json
                ? JsonContentFormatter.Format(content)
                : content;

            throw new RestAssertionException(
                "HTTP response code",
                StatusCodeFormatter.Format(expectedCode),
                StatusCodeFormatter.Format(actualCode),
                $"HTTP response content was:{NewLine}{formattedContent}");
        }

        private static void JsonContentShouldBeEqual(object expectedContent, object actualContent)
        {
            var expectedJsonLines = JsonContentFormatter.Format(expectedContent).Split(NewLine);
            var actualJsonLines = JsonContentFormatter.Format(actualContent).Split(NewLine);

            for (var i = 0; i < actualJsonLines.Length; i++)
            {
                if (actualJsonLines[i] == expectedJsonLines[i])
                {
                    continue;
                }

                actualJsonLines[i] = '*' + actualJsonLines[i].Substring(1);

                if (expectedJsonLines.Length > i)
                {
                    expectedJsonLines[i] = '*' + expectedJsonLines[i].Substring(1);
                }

                throw new RestAssertionException(
                    "HTTP response content",
                    string.Join(NewLine, expectedJsonLines),
                    string.Join(NewLine, actualJsonLines),
                    $"Difference: Line {i + 1}");
            }
        }
    }
}
