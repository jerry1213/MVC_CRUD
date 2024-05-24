namespace Platform.Utility.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 取得西元年月日
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="hasSplitChar">是否要分隔符號"-"</param>
        /// <returns></returns>
        public static string ToYYYYMMDD(this DateTime dt, bool hasSplitChar = true)
        {
            if (dt.Equals(DateTime.MinValue) || dt.Equals(DateTime.MaxValue)) return "";
            return dt.ToString(hasSplitChar ? "yyyy-MM-dd" : "yyyyMMdd");
        }
        public static string ToYYYYMMDD(this DateTime? dt, bool hasSplitChar = true)
        {
            return dt == null ? "" : dt.Value.ToYYYYMMDD(hasSplitChar);
        }

        /// <summary>
        /// 取得西元年月日時分秒 輸出2020-01-07 12:34:56
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToYYYYMMDDHHMMSS(this DateTime dt)
        {
            if (dt.Equals(DateTime.MinValue) || dt.Equals(DateTime.MaxValue)) return "";
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string ToYYYYMMDDHHMMSS(this DateTime? dt)
        {
            return dt == null ? "" : dt.Value.ToYYYYMMDDHHMMSS();
        }

        public static DateTime? ToDateTime(this string? dtString)
        {
            if (string.IsNullOrEmpty(dtString)) return null;
            return DateTime.Parse(dtString);
        }
    }
}
