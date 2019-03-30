namespace RestAssertions
{
    using System;

    using static Formatters.FormatUtils;

    public class RestAssertionException : Exception
    {
        public RestAssertionException(string what, object toBe, object butWas, string additionalInfo)
            : base($"{NewLine}{NewLine}" +
                   $"Expected {what} to be:{NewLine}" +
                   $"{toBe}{NewLine}{NewLine}" +
                   $"but was:{NewLine}" +
                   $"{butWas}{NewLine}{NewLine}" +
                   $"{additionalInfo}{NewLine}")
        {
        }

        public RestAssertionException(string error)
            : base(error)
        {
        }
    }
}
