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
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string @class = "")
        {
            return MvcHtmlString.Create(String.Format("<img src='{0}' class='{1}'/>", VirtualPathUtility.ToAbsolute(src), @class));
        }
    }
}