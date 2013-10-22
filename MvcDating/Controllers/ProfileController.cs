using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDating.Models;
using MvcDating.Filters;
using AutoMapper;


namespace MvcDating.Controllers
{
    [InitializeSimpleMembership]
    public class ProfileController : Controller
    {
        private UsersContext db = new UsersContext();

        //
        // GET: /Profile/

        public ActionResult Index(string username = "")
        {
            // Get profile information
            var profile = (from p in db.Profiles where p.UserName == username select p).SingleOrDefault();
            if (profile == null) throw new HttpException(404, "Profile not found");

            // Add a visitor
            /*if (profile.UserId == WebSecurity.CurrentUserId)
            {
                db.Visitors.AddOrUpdate(v => v.VisitorId, new Visitor()
                {
                    UserId = profile.UserId,
                    VisitorId = WebSecurity.CurrentUserId,
                    Timestamp = DateTime.Now
                });
            }*/

            Mapper.CreateMap<MvcDating.Models.Profile, ProfileView>();
            var profileView = Mapper.Map<MvcDating.Models.Profile, ProfileView>(profile);

            return View(profileView);
        }

        //
        // GET: /Profile/Edit/5

        public ActionResult Edit(string username = "")
        {
            var profile = (from p in db.Profiles where p.UserName == username select p).SingleOrDefault();

            Mapper.CreateMap<MvcDating.Models.Profile, ProfileView>();
            var profileview = Mapper.Map<MvcDating.Models.Profile, ProfileView>(profile);

            profileview.SituationItems = from p in profileview.SituationItems
                                         select new SelectListItem
                                         {
                                             Selected = (profile.Situation.ToString() == p.Value) ? true : false,
                                             Text = p.Text,
                                             Value = p.Value
                                         };

            return View(profileview);
        }

        //
        // POST: /Profile/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileView profileview)
        {
            Mapper.CreateMap<ProfileView, MvcDating.Models.Profile>();
            var profile = Mapper.Map<ProfileView, MvcDating.Models.Profile>(profileview);

            if (ModelState.IsValid)
            {
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profileview);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}