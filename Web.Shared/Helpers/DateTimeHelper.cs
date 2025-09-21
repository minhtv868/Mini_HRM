using System.Globalization;

namespace Web.Shared.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime StrToDateTime(this string input,
                  string format = "dd/MM/yyyy")
        {
            try
            {
                var resultVar = DateTime.ParseExact(
                    s: input,
                    format: format,
                    provider: CultureInfo.CurrentCulture);

                return resultVar;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CultureNotFoundException)
            {
                throw; // Given Culture is not supported culture
            }
        }
        public static DateTime StrToDateTime2(this string input, string format = "dd-MM-yyyy HH:mm")
        {
            try
            {
                return DateTime.ParseExact(
                    input,
                    format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None
                );
            }
            catch (FormatException ex)
            {
                throw new Exception(
                    $"Chuỗi ngày giờ '{input}' không khớp với format '{format}'",
                    ex
                );
            }
        }

        //public static DateTime ToDateTime(this string input,
        //          string format = "dd-MM-yyyy")
        //{
        //    DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultVar);

        //    return resultVar;
        //}

        public static string ToStringFormat(this DateTime dateTime, string format = "dd/MM/yyyy HH:mm")
        {
            if (dateTime != DateTime.MinValue)
            {
                return dateTime.ToString(format);
            }

            return string.Empty;
        }

        public static string ToStringFormat(this DateTime? dateTime, string format = "dd/MM/yyyy HH:mm")
        {
            if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
            {
                return dateTime.Value.ToString(format);
            }

            return string.Empty;
        }

        public static string ToString(this DateTime dt, string textEmpty = "", string format = "dd/MM/yyyy")
        {
            return dt == DateTime.MinValue ? textEmpty : dt.ToString(format);
        }

        public static string DateTimeToString(this DateTime? dt, string textEmpty = "", string format = "dd/MM/yyyy")
        {
            return (!dt.HasValue || dt.Value == DateTime.MinValue) ? textEmpty : dt.Value.ToString(format);
        }
        public static string TimeAgo(this DateTime dateTime)
        {
            if (dateTime != DateTime.MinValue)
            {
                TimeSpan span = DateTime.Now - dateTime;
                if (span.Days <= 0)
                {
                    if (span.Hours > 0)
                        return string.Format(" {0} {1} trước",
                            span.Hours, "giờ");
                    if (span.Minutes > 0)
                        return string.Format(" {0} {1} trước",
                            span.Minutes, "phút");
                    if (span.Seconds > 5)
                        return string.Format(" {0} giây trước", span.Seconds);
                    if (span.Seconds <= 5)
                        return "vừa xong";
                }
                else if (span.Days <= 30)
                {
                    return string.Format(" {0} ngày trước", span.Days);
                }
                return dateTime.ToStringFormat();
            }
            return string.Empty;
        }
        public static string TimeAgo(this DateTime? dateTime)
        {
            if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
            {
                TimeSpan span = DateTime.Now - dateTime.Value;
                if (span.Days <= 0)
                {
                    if (span.Hours > 0)
                        return string.Format(" {0} {1} trước",
                            span.Hours, "giờ");
                    if (span.Minutes > 0)
                        return string.Format(" {0} {1} trước",
                            span.Minutes, "phút");
                    if (span.Seconds > 5)
                        return string.Format(" {0} giây trước", span.Seconds);
                    if (span.Seconds <= 5)
                        return "vừa xong";
                }
                else if (span.Days <= 30)
                {
                    return string.Format(" {0} ngày trước", span.Days);
                }
                return dateTime.ToStringFormat();
            }
            return string.Empty;
        }
        public static string TimeLeft(this DateTime dt)
        {
            TimeSpan span = dt - DateTime.Now;
            if (span.Days <= 0)
            {
                if (span.Hours > 0)
                    return $"Trong {span.Hours}{"h"} nữa";
                if (span.Minutes > 0)
                    return $"Trong {span.Minutes}{"'"} nữa";
                if (span.Seconds > 5)
                    return $"Trong {span.Seconds}s nữa";
                if (span.Seconds <= 5)
                    return "Trong vài giây";
            }
            return dt.ToStringFormat();
        }
        public static string GetDateHH24(object m_DateTime)
        {
            string RetVal = "";
            try
            {

                RetVal = DateTime.Parse(m_DateTime.ToString()) == DateTime.MinValue ? "" : DateTime.Parse(m_DateTime.ToString()).ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
            }
            return RetVal;
        }
        public static DateTime ToDateTime(this string input,
               string format = "dd-MM-yyyy")
        {
            if (string.IsNullOrWhiteSpace(input))
                return DateTime.MinValue;

            input = input.Replace("/", "-");
            DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultVar);

            return resultVar;
        }
    }
}
