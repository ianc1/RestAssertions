namespace RestAssertions.Formatters
{
    using System.Collections.Generic;
    using System.Linq;

    using static FormatUtils;

    internal static class HeaderFormatter
    {
        public static string Format(IList<string> values)
        {
            if (!values.Any())
            {
                return $"{Indent}null";
            }

            return $"{Indent}{string.Join(", ", values)}";
        }

        public static string Format(Dictionary<string, IEnumerable<string>> headers)
        {
            var orderedHeaders = new SortedDictionary<string, IEnumerable<string>>(headers);

            return string.Join(NewLine, orderedHeaders.OrderBy(header => header.Key).Select((header, index) => $"{index + 1, 6}:  {header.Key}: {string.Join(", ", header.Value)}"));
        }
    }
}
