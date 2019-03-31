namespace RestAssertions.Formatters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mime;

    using Microsoft.Net.Http.Headers;

    using static FormatUtils;

    public static class HttpResponseFormatter
    {
        public static string Format(
            HttpStatusCode statusCode,
            Dictionary<string, IEnumerable<string>> headers,
            string content)
        {
            var formattedContent = IsJsonContent(headers)
                ? JsonContentFormatter.Format(content)
                : content;

            return $"Status:{NewLine}{StatusCodeFormatter.Format(statusCode)}{NewLine}{NewLine}"
                 + $"Headers:{NewLine}{HeaderFormatter.Format(headers)}{NewLine}{NewLine}"
                 + $"Content:{NewLine}{formattedContent}";
        }

        private static bool IsJsonContent(Dictionary<string, IEnumerable<string>> headers)
        {
            if (!headers.TryGetValue(HeaderNames.ContentType, out var contentType))
            {
                return false;
            }

            return contentType.Any(value => value.StartsWith(MediaTypeNames.Application.Json));
        }
    }
}
