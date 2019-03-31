namespace RestAssertions
{
    using System;

    using static RestAssertions.Formatters.FormatUtils;

    public class RestAssertionException : Exception
    {
        public RestAssertionException(string what, object toBe, object butWas, string additionalInfo)
            : base($"{NewLine}{NewLine}" +
                   $"Expected {what} to be:{NewLine}" +
                   $"{toBe}{NewLine}{NewLine}" +
                   $"but was:{NewLine}" +
                   $"{butWas}{NewLine}{NewLine}" +
                   $"Additional information{NewLine}{NewLine}{additionalInfo}{NewLine}{NewLine}")
        {
        }

        public RestAssertionException(string error, string additionalInfo)
            : base($"{error}{NewLine}{NewLine}" +
                   $"Additional information{NewLine}{NewLine}{additionalInfo}{NewLine}{NewLine}")
        {
        }
    }
}
