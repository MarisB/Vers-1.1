using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ControlitFactory.Support
{
    public class DecimalConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0";
            decimal thedecimal = (decimal)value;
            return thedecimal.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (string.IsNullOrEmpty(strValue))
                strValue = "0";
            decimal resultdecimal;
            if (decimal.TryParse(strValue, out resultdecimal))
            {
                return resultdecimal;
            }
            return 0;
        }

    }

    /// <summary>
    /// Implements a (helper) class to convert date and time values.
    /// </summary>
    public static class DateTimeConverter
    {

        /// <summary>
        /// Converts from DateTime to TimaSpan.
        /// </summary>
        /// <param name="dt">The source DateTime value.</param>
        /// <returns>Returns the time portion of DateTime in the form of TimeSpan if succeeded, null otherwise.</returns>
        public static TimeSpan? DateTimeToTimeSpan(DateTime dt)
        {
            TimeSpan FResult;
            try
            {
                FResult = dt - dt.Date;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }

            return FResult;
        }


        /// <summary>
        /// Converts from Timespan to DateTime.
        /// </summary>
        /// <param name="ts">The source TimeSpan value.</param>
        /// <returns>Returns a DateTime filled with date equals to mindate and time equals to time in timespan if succeeded, null otherwise.</returns>
        public static DateTime? TimeSpanToDateTime(TimeSpan ts)
        {
            DateTime? FResult = null;
            try
            {
                string year = string.Format("{0:0000}", DateTime.MinValue.Date.Year);
                string month = string.Format("{0:00}", DateTime.MinValue.Date.Month);
                string day = string.Format("{0:00}", DateTime.MinValue.Date.Day);

                string hours = string.Format("{0:00}", ts.Hours);
                string minutes = string.Format("{0:00}", ts.Minutes);
                string seconds = string.Format("{0:00}", ts.Seconds);

                string dSep = "-"; string tSep = ":"; string dtSep = "T";

                // yyyy-mm-ddTHH:mm:ss
                string dtStr = string.Concat(year, dSep, month, dSep, day, dtSep, hours, tSep, minutes, tSep, seconds);

                DateTime dt;
                if (DateTime.TryParseExact(dtStr, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dt))
                {
                    FResult = dt;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }

            return FResult;
        }


        /// <summary>
        /// Converts from DateTime to DateTimeOffSet.
        /// </summary>
        /// <param name="dt">The source DateTime to convert.</param>
        /// <returns>Returns a DateTimeOffSet if succeeded, null otherwise.</returns>
        public static DateTimeOffset? DateTimeToDateTimeOffSet(DateTime dt)
        {
            try
            {
                return new DateTimeOffset(dt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Converts from DateTimeOffSet to DateTime.
        /// </summary>
        /// <param name="dto">The source DateTimeOffSet to convert.</param>
        /// <returns>Returns a DateTime if succeeded, null otherwise.</returns>
        public static DateTime? DateTimeOffSetToDateTime(DateTimeOffset dto)
        {
            try
            {
                return dto.DateTime;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }

    /// <summary>
    /// Implements a databind converter from DateTime to DateTimeOffset.
    /// </summary>
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                DateTime date = (DateTime)value;
                DateTimeOffset? dto = DateTimeConverter.DateTimeToDateTimeOffSet(date);
                return dto.GetValueOrDefault(DateTimeOffset.MinValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return DateTimeOffset.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                DateTimeOffset dto = (DateTimeOffset)value;
                return dto.DateTime;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return DateTime.MinValue;
            }
        }
    }

    /// <summary>
    /// Implements a databind converter to convert from DateTime to TimeSpan and vice-versa.
    /// </summary>
    public class DateTimeToTimeSpanConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                DateTime dt = (DateTime)value;
                TimeSpan? ts = DateTimeConverter.DateTimeToTimeSpan(dt);
                return ts.GetValueOrDefault(TimeSpan.MinValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return TimeSpan.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                TimeSpan ts = (TimeSpan)value;
                DateTime? dt = DateTimeConverter.TimeSpanToDateTime(ts);
                return dt.GetValueOrDefault(DateTime.MinValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return DateTime.MinValue;
            }
        }
    }

}
