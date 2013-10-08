using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcDating.Helpers
{
    /**
     * Extends @Html class with new methods
     */
    public static class Extensions
    {
        public static string Label(this HtmlHelper helper, string target, string text)
        {
            return String.Format("<label for='{0}'>{1}</label>", target, text);
        }
        public static string Image(this HtmlHelper helper, string src, string @class = "picture")
        {
            return String.Format("<img src='{0}' class='{1}'/>", src, @class);
        }
    }
}