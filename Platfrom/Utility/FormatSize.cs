namespace Platform.Utility
{
    public class FormatSize
    {
        public static string ByteToString(double size)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }
    }
}
