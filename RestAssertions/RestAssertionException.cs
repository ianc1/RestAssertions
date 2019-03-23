namespace RestAssertions
{
    using System;

    public class RestAssertionException : Exception
    {
        public RestAssertionException(string what, object toBe, object butWas, string additionalInfo)
            : base($"\r\n\r\nExpected {what} to be:\r\n{toBe}\r\n\r\nbut was:\r\n{butWas}\r\n\r\n{additionalInfo}")
        {
        }
    }
}
