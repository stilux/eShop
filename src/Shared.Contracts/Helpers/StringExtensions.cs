using System.Text.RegularExpressions;

namespace Shared.Contracts.Helpers
{
    public static class StringExtensions
    {
        public static string PascalToKebabCaseMessage(this string value)
        {
            return PascalToKebabCase(value,"message");           
        }
        public static string PascalToKebabCaseActivity(this string value)
        {
            return PascalToKebabCase(value, "activity");
        }
        private static string PascalToKebabCase(string value,string postfix)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var result = Regex.Replace(
                    value,
                    "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                    "-$1",
                    RegexOptions.Compiled)
                .Trim()
                .ToLower();

            var segments = result.Split('-');
            if (segments[segments.Length - 1] != postfix)
                return result;
            return result.Substring(0, result.Length - 8);
        }
    }
}