namespace RestAssertions.Formatters
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class HeaderFormatter
    {
        public static string Format(IList<string> values)
        {
            if (!values.Any())
            {
                return "null";
            }

            return string.Join(", ", values);
        }
    }
}
