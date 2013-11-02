using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDating.Models;
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
            var userOnlineDate = System.Web.HttpRuntime.Cache[userId + "_onlineDate"];

            if (userOnlineDate != null)
            {
                var isOnline = DateTime.Now.Subtract(DateTime.Parse(userOnlineDate.ToString())).TotalMinutes < 15;
                return isOnline;
            }

            return false;
        }

    }
}