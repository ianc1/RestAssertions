namespace RestAssertions.Formatters
{
    using System.Collections.Generic;
    using System.Linq;

    public static class HeaderFormatter
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
