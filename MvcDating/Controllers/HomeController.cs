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

            ViewBag.userPictures = (from p in db.Pictures where p.IsAvatar select p.Thumb).Take(5);

            return View();
        }
    }
}
