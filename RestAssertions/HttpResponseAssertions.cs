namespace RestAssertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Formatters;
    using Microsoft.Net.Http.Headers;
    using Utilities;
    using static Formatters.FormatUtils;

    public class HttpResponseAssertions
    {
        private readonly HttpStatusCode statusCode;
        private readonly HttpContentHeaders contentHeaders;
        private readonly string content;

        private HttpResponseAssertions(HttpStatusCode statusCode, HttpContentHeaders contentHeaders, string content)
        {
            this.statusCode = statusCode;
            this.contentHeaders = contentHeaders;
            this.content = content;
        }

        public static async Task<HttpResponseAssertions> Create(HttpResponseMessage httpResponseMessage)
        {
            return new HttpResponseAssertions(
                httpResponseMessage.StatusCode,
                httpResponseMessage.Content.Headers,
                await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public void ShouldBe(HttpStatusCode expectedStatusCode)
        {
            if (expectedStatusCode == statusCode)
            {
                return;
            }

            var formattedContent = contentHeaders.ContentType.MediaType == MediaTypeNames.Application.Json
                ? JsonContentFormatter.Format(content)
                : content;

            throw new RestAssertionException(
                "HTTP response code",
                StatusCodeFormatter.Format(expectedStatusCode),
                StatusCodeFormatter.Format(statusCode),
                $"HTTP response content was:{NewLine}{formattedContent}");
        }

        public void ShouldHaveHeader(string name, string value)
        {
            var actualValues = (contentHeaders.TryGetValues(name, out var values)
                ? values
                : Enumerable.Empty<string>()).ToList();

            var actualValuesWithoutCharset = new List<string>();

            foreach (var actualValue in actualValues)
            {
                actualValuesWithoutCharset.Add(actualValue.Split(';')[0]);
            }

            if (actualValuesWithoutCharset.Contains(value, StringComparer.InvariantCultureIgnoreCase))
            {
                return;
            }

            var formattedContent = contentHeaders.ContentType.MediaType == MediaTypeNames.Application.Json
                ? JsonContentFormatter.Format(content)
                : content;

            throw new RestAssertionException(
                $"HTTP response header \"{name}\"",
                value,
                string.Join(", ", actualValuesWithoutCharset),
                $"HTTP response content was:{NewLine}{formattedContent}");
        }

        public void ShouldMatchJson(object expectedContent)
        {
            ShouldHaveHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json);

            var actualContent = JsonUtils.Deserialize<dynamic>(content);

            JsonContentShouldBeEqual(expectedContent, actualContent);
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
