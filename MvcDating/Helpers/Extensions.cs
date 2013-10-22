using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace MvcDating.Helpers
{
    /**
     * Extends @Html class with new methods
     */
    public static class Extensions
    {
        private static string GetContentPath(string src)
        {
            return src.Contains(@"//") ? src : VirtualPathUtility.ToAbsolute(src);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string src, string @class = "")
        {
            return MvcHtmlString.Create(String.Format("<img src='{0}' class='{1}'/>", GetContentPath(src), @class));
        }

        public static MvcHtmlString ImageLink(this HtmlHelper helper, string urlSrc, MvcHtmlString htmlImage, string @class = "")
        {
            return MvcHtmlString.Create(String.Format("<a href='{0}' class='{1}'>" + htmlImage + "</a>", 
                urlSrc,
                @class));
        }
    }
}