using System;

namespace GoldMountainApi.Controllers.Helper
{
    public static class DateTimeExtentions
    {
        public static DateTime EndOfTheDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static bool IsInThisMonth(this DateTime date)
        {
            var now = DateTime.Today;
            return (date > now.EndOfTheDay().AddDays(-now.Day) && date.EndOfTheDay() <= now);
        }

        public static bool IsInLast30Days(this DateTime date)
        {
            var now = DateTime.Today;
            return (date > now.EndOfTheDay().AddDays(-31) && date.EndOfTheDay() <= now);
        }

        public static bool IsInLast6Months(this DateTime date)
        {
            var now = DateTime.Today;
            return (date > now.EndOfTheDay().AddMonths(-6) && date.EndOfTheDay() <= now);
        }
    }
}
