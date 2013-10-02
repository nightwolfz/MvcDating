using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MvcDating
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config) {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // Fixes a weird error of API not returning anything but errors
            // See: http://social.msdn.microsoft.com/Forums/vstudio/en-US/a5adf07b-e622-4a12-872d-40c753417645/web-api-error-the-objectcontent1-type-failed-to-serialize-the-response-body-for-content
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}