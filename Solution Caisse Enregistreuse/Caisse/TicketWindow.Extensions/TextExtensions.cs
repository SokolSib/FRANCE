using System;

namespace TicketWindow.Extensions
{
    public static class TextExtensions
    {
        public static double ToDouble(this string text)
        {
            return double.Parse(text.Replace('.', ','));
        }

        public static decimal ToDecimal(this string text)
        {
            return decimal.Parse(text.Replace('.', ','));
        }

        public static byte ToByte(this string text)
        {
            return byte.Parse(text);
        }

        public static Guid? ToNullableGuid(this string text)
        {
            if (!string.IsNullOrEmpty(text))
                return ToGuid(text);
            return null;
        }

        public static bool? ToNullableBool(this string text)
        {
            if (!string.IsNullOrEmpty(text))
                return ToBool(text);
            return null;
        }

        public static decimal? ToNullableDecimal(this string text)
        {
            if (!string.IsNullOrEmpty(text))
                return ToDecimal(text);
            return null;
        }

        public static Guid ToGuid(this string text)
        {
            return Guid.Parse(text);
        }

        public static short ToShort(this string text)
        {
            return short.Parse(text);
        }

        public static int ToInt(this string text)
        {
            return int.Parse(text);
        }

        public static int? ToNullableInt(this string text)
        {
            if (!string.IsNullOrEmpty(text))
                return ToInt(text);
            return null;
        }

        public static DateTime ToDateTime(this string text)
        {
            return DateTime.Parse(text);
        }

        public static DateTime? ToNullableDateTime(this string text)
        {
            if (!string.IsNullOrEmpty(text))
                return ToDateTime(text);
            return null;
        }

        public static bool ToBool(this string text)
        {
            return bool.Parse(text);
        }
    }
}