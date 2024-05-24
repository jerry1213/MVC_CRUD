namespace Platform.Utility.Extensions
{
    public static class StringExtension
    {
        public static string SafeTrim(this string? s)
        {
            return s == null ? string.Empty : s.Trim();
        }
    }
}
