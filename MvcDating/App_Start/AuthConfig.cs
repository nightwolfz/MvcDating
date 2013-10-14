using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using MvcDating.Models;

namespace MvcDating
{
    public static class AuthConfig
    {
        public static void RegisterAuth() {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "577772775614658",
                appSecret: "f4967e9d2fe74285af6f92f05555aa93");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
