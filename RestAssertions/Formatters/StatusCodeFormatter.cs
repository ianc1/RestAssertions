namespace RestAssertions.Formatters
{
    using System.Net;

    internal static class StatusCodeFormatter
    {
        public static string Format(HttpStatusCode statusCode)
        {
            return $"{(int)statusCode} {statusCode}";
        }
    }
}
