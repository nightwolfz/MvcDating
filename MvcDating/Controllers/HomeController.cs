using System.Web.Security;
using MvcDating.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcDating.Controllers
{
    public class HomeController : Controller
    {
        private UsersContext db = new UsersContext();

        public ActionResult Index()
        {
            var query = from profile in db.Profiles
                        join picture in db.Pictures on profile.UserId equals picture.UserId where picture.IsAvatar
                        select new FeaturedView { UserName = profile.UserName, Thumb = picture.Thumb };

            ViewBag.featuredUsers = query.Take(5);

            return View();
        }
    }
}
