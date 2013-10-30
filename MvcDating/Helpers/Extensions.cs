using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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

        /**
         * Generate an image tag
         */
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string @class = "", object htmlAttributes = null)
        {
            var tag = new TagBuilder("img");
            tag.MergeAttribute("src", GetContentPath(src), true);
            tag.MergeAttribute("class", @class, true);
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        /**
         * Generate a <a><img/></a> tag
         */
        public static MvcHtmlString ImageLink(this HtmlHelper helper, string href, MvcHtmlString htmlImage, object htmlAttributes = null)
        {
            var tag = new TagBuilder("a");
            tag.MergeAttribute("href", GetContentPath(href), true);
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.InnerHtml += htmlImage;
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        // Not working somehow
        public static MvcHtmlString DisplayAvatar(this HtmlHelper helper, string userName, string thumbSrc)
        {
            var url = new UrlHelper(helper.ViewContext.RequestContext);
            var urlRoute = url.RouteUrl(routeValues: new
            {
                Action = "Index",
                Controller = "Profile",
                username = userName
            });
            return ImageLink(helper, urlRoute, Image(helper, thumbSrc, htmlAttributes: new { @onerror = "this.src='Uploads/default.png';" }));
        }
    }
}