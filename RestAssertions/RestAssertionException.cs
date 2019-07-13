namespace RestAssertions
{
    using System;

    using static RestAssertions.Formatters.FormatUtils;

    public class RestAssertionException : Exception
    {
        public const string ToBe = "to be";

        public const string ToContain = "to contain";

        public RestAssertionException(string what, object expectedValue, object actualValue, string additionalInfo, string expectationType = ToBe)
            : base($"{NewLine}{NewLine}" +
                   $"Expected {what} {expectationType}:{NewLine}" +
                   $"{expectedValue}{NewLine}{NewLine}" +
                   $"but was:{NewLine}" +
                   $"{actualValue}{NewLine}{NewLine}" +
                   $"Additional information{NewLine}{NewLine}{additionalInfo}{NewLine}{NewLine}")
        {
        }

        public RestAssertionException(string error, string additionalInfo)
            : base($"{NewLine}{NewLine}" +
                   $"{error}{NewLine}{NewLine}" +
                   $"Additional information{NewLine}{NewLine}{additionalInfo}{NewLine}{NewLine}")
        {
        }
    }
}
