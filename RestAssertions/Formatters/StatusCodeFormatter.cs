namespace RestAssertions.Formatters
{
    using System.Net;

    public static class StatusCodeFormatter
    {
        public static string Format(HttpStatusCode statusCode)
        {
            return $"{(int)statusCode} {statusCode}";
        }
    }
}
