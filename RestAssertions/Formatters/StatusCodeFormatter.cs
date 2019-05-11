namespace RestAssertions.Formatters
{
    using System.Net;
    using Microsoft.AspNetCore.WebUtilities;

    using static FormatUtils;

    internal static class StatusCodeFormatter
    {
        public static string Format(HttpStatusCode statusCode)
        {
            var message = ReasonPhrases.GetReasonPhrase((int)statusCode);

            return $"{Indent}{(int)statusCode} {message}";
        }
    }
}
