namespace RestAssertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Formatters;
    using Microsoft.Net.Http.Headers;
    using Utilities;
    using static Formatters.FormatUtils;

    public class HttpResponseAssertions
    {
        private readonly HttpStatusCode statusCode;
        private readonly Dictionary<string, IEnumerable<string>> headers;
        private readonly string content;

        private HttpResponseAssertions(HttpStatusCode statusCode, Dictionary<string, IEnumerable<string>> headers, string content)
        {
            this.statusCode = statusCode;
            this.headers = headers;
            this.content = content;
        }

        public static async Task<HttpResponseAssertions> Create(HttpResponseMessage httpResponseMessage)
        {
            var headers = httpResponseMessage.Headers
                .Concat(httpResponseMessage.Content.Headers)
                .ToDictionary(k => k.Key, v => v.Value);

            return new HttpResponseAssertions(
                httpResponseMessage.StatusCode,
                headers,
                await httpResponseMessage.Content.ReadAsStringAsync());
        }

        public void ShouldBe(HttpStatusCode expectedStatusCode)
        {
            if (expectedStatusCode == statusCode)
            {
                return;
            }

            var contentType = GetHeaderValues(HeaderNames.ContentType).FirstOrDefault();

            var formattedContent = contentType == MediaTypeNames.Application.Json
                ? JsonContentFormatter.Format(content)
                : content;

            throw new RestAssertionException(
                "HTTP response code",
                StatusCodeFormatter.Format(expectedStatusCode),
                StatusCodeFormatter.Format(statusCode),
                $"HTTP response content was:{NewLine}{formattedContent}");
        }

        public void ShouldHaveHeader(string expectedName, string expectedValue)
        {
            var actualValues = GetHeaderValues(expectedName).ToList();

            if (actualValues.Contains(expectedValue, StringComparer.InvariantCultureIgnoreCase))
            {
                return;
            }

            var contentType = GetHeaderValues(HeaderNames.ContentType).FirstOrDefault();

            var formattedContent = contentType == MediaTypeNames.Application.Json
                ? JsonContentFormatter.Format(content)
                : content;

            throw new RestAssertionException(
                $"HTTP response header \"{expectedName}\"",
                expectedValue,
                HeaderFormatter.Format(actualValues),
                $"HTTP response content was:{NewLine}{formattedContent}");
        }

        public void ShouldMatchJson(object expectedContent)
        {
            ShouldHaveHeader(HeaderNames.ContentType, $"{MediaTypeNames.Application.Json}; charset=utf-8");

            var actualContent = JsonUtils.Deserialize<dynamic>(content);

            JsonContentShouldBeEqual(expectedContent, actualContent);
        }

        public int ShouldContainLocationHeaderWithId()
        {
            if (int.TryParse(GetLocationHeaderLastSegment(), out var id))
            {
                return id;
            }

            throw new RestAssertionException("Expected response to contain a location header with an integer id but did not.");
        }

        public Guid ShouldContainLocationHeaderWithGuid()
        {
            if (Guid.TryParse(GetLocationHeaderLastSegment(), out var id))
            {
                return id;
            }

            throw new RestAssertionException("Expected response to contain a location header with a GUID but did not.");
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

        private IEnumerable<string> GetHeaderValues(string name)
        {
            return headers.TryGetValue(name, out var values) ? values : Enumerable.Empty<string>();
        }

        private string GetLocationHeaderLastSegment()
        {
            var location = GetHeaderValues(HeaderNames.Location).FirstOrDefault();

            var id = location?.Split('/').Last();

            if (string.IsNullOrEmpty(id))
            {
                throw new RestAssertionException("Expected response to contain a location header but did not.");
            }

            return id;
        }
    }
}
