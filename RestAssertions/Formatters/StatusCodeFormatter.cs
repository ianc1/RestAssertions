namespace RestAssertions.Formatters
{
    using System.Net;

    using static FormatUtils;

    internal static class StatusCodeFormatter
    {
        public static string Format(HttpStatusCode statusCode)
        {
            return $"{Indent}{(int)statusCode} {statusCode}";
        }
    }
}
