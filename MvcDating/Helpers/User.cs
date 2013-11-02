using System;
using System.Web;
using WebMatrix.WebData;

namespace MvcDating.Helpers
{
    public class User
    {
        /// <summary>
        /// Sets the current user as online (using cache)
        /// </summary>
        public static void SetOnline()
        {
            // Set current user as online
            if (WebSecurity.IsAuthenticated)
            {
                HttpRuntime.Cache[WebSecurity.CurrentUserId + "_onlineDate"] = DateTime.Now;
            }
        }

        /// <summary>
        /// Get current user online status (using cache)
        /// </summary>
        public static bool GetOnlineStatus(int userId)
        {
            var userOnlineDate = HttpRuntime.Cache[userId + "_onlineDate"];

            if (userOnlineDate == null) return false;

            var isOnline = DateTime.Now.Subtract(DateTime.Parse(userOnlineDate.ToString())).TotalMinutes < 15;
            return isOnline;
        }

    }
}