using System;
using System.Globalization;

namespace MvcDating.Helpers
{
    public class Time
    {
        /// <summary>
        /// Facebook like "time ago"
        /// </summary>
        public static string GetTimeAgo(DateTime timestamp)
        {
            TimeSpan diff = DateTime.Now.Subtract(timestamp);

            if (diff.TotalDays > 1 && diff.TotalDays < 4)   return TimeAgoFormat(diff.TotalDays, "day");
            if (diff.TotalHours > 1)                        return TimeAgoFormat(diff.TotalHours, "hour");
            if (diff.TotalMinutes > 1)                      return TimeAgoFormat(diff.TotalMinutes, "minute");
            if (diff.TotalMinutes < 1)                      return "A few seconds ago";
            
            return timestamp.ToString("MMM dd, yyyy");
        }

        private static string TimeAgoFormat(double time, string timeUnit)
        {
            return String.Format("{0} {1}{2} ago", (int)time, timeUnit, (time >= 2 ? "s" : ""));
        }


        /// <summary>
        /// Get age based on birthday
        /// </summary>
        public static int GetAge(DateTime birthday)
        {
            var now = DateTime.Today;
            int age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age)) age--;

            return age;
        }
    }
}