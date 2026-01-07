using System.Text.RegularExpressions;

namespace SafeVault.Services
{
    public static class InputSanitizer
    {
        public static string Clean(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Remove HTML tags (basic XSS mitigation)
            input = Regex.Replace(input, "<.*?>", string.Empty, RegexOptions.Singleline);

            // Normalize whitespace
            input = Regex.Replace(input, @"\s+", " ");

            // Remove some common SQL injection characters/patterns
            var blackList = new[]
            {
                "'", "\"", "--", ";", "/*", "*/", "xp_"
            };

            foreach (var item in blackList)
            {
                input = input.Replace(item, string.Empty, StringComparison.OrdinalIgnoreCase);
            }

            return input.Trim();
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Simplified email pattern for demo purposes
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            // Only letters, numbers, underscore, 3-30 chars
            var pattern = @"^[a-zA-Z0-9_]{3,30}$";
            return Regex.IsMatch(username, pattern);
        }
    }
}
