using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MvcDating.Filters;
using MvcDating.Models;
using WebMatrix.WebData;

namespace MvcDating.Controllers
{
    [InitializeSimpleMembership]
    public class VisitorsController : Controller
    {
        private UsersContext db = new UsersContext();

        //
        // GET: /Visitors/

        public ActionResult Index()
        {
            var visitorView = from visitor in db.Visitors
                           join profile in db.Profiles on visitor.VisitorId equals profile.UserId
                           join picture in db.Pictures on profile.UserId equals picture.UserId
                           where profile.UserId == WebSecurity.CurrentUserId
                           select new VisitorView()
                           {
                               UserId = visitor.UserId,
                               Timestamp = visitor.Timestamp,
                               Profile = profile,
                               Thumb = picture.Thumb
                           };

            return View(visitorView);
        }


        public ActionResult History()
        {
            var visitorView = from visitor in db.Visitors
                              join profile in db.Profiles on visitor.VisitorId equals profile.UserId
                              join picture in db.Pictures on profile.UserId equals picture.UserId
                              where visitor.VisitorId == WebSecurity.CurrentUserId
                              select new VisitorView()
                              {
                                  UserId = visitor.UserId,
                                  Timestamp = visitor.Timestamp,
                                  Profile = profile,
                                  Thumb = picture.Thumb
                              };

            return View(visitorView);
        }
    }
}
