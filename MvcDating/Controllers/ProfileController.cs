using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Domain.Filters;
using MvcDating.Models;
using MvcDating.Services;
using Domain.Models;
using Profile = Domain.Models.Profile;


namespace MvcDating.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class ProfileController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        // GET: /Profile/
        public ActionResult Index(string username = "")
        {
            // Get profile information
            var profile = db.Profiles.Single(p => p.UserName == username);
            if (profile == null) throw new HttpException(404, "Profile not found");

            // Add a visitor
            db.Visitors.AddOrUpdateVisitor(profile.UserId);

            Mapper.CreateMap<Profile, ProfileView>();
            var profileView = Mapper.Map<Profile, ProfileView>(profile);

            return View(profileView);
        }


        // GET: /Profile/Edit/5
        public ActionResult Edit(string username = "")
        {
            var profile = db.Profiles.Single(p => p.UserName == username);

            Mapper.CreateMap<Profile, ProfileView>();
            var profileview = Mapper.Map<Profile, ProfileView>(profile);

            return View(profileview);
        }

        // POST: /Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileView profileview)
        {
            Mapper.CreateMap<ProfileView, Profile>()
                .ForMember(src => src.Email, opt => opt.Ignore());

            var profile = Mapper.Map<ProfileView, Profile>(profileview);
            profile.UpdatedDate = DateTime.Now;

            // Set this user as online
            Helpers.User.SetOnline();

            if (ModelState.IsValid)
            {
                db.Profiles.Update(profile);
                db.Profiles.Context.Entry(profile).Property(x => x.Email).IsModified = false;
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