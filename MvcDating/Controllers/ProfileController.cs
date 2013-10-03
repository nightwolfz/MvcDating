using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
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
            var profile = (from p in db.Profiles where p.UserName == username select p).SingleOrDefault();

            Mapper.CreateMap<MvcDating.Models.Profile, ProfileView>();
            var profileview = Mapper.Map<MvcDating.Models.Profile, ProfileView>(profile);

            return View(profileview);
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
            if (ModelState.IsValid)
            {
                db.Entry(profileview).State = EntityState.Modified;
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