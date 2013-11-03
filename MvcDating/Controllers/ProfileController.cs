using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDating.Models;
using MvcDating.Filters;
using AutoMapper;
using MvcDating.Services;
using WebMatrix.WebData;


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
            if (profile.UserId != WebSecurity.CurrentUserId)
            {
                var visitor = db.Visitors.Single(dto => dto.VisitorId == WebSecurity.CurrentUserId);

                if (visitor == null)
                {
                    db.Visitors.Add(new Visitor
                    {
                        UserId = profile.UserId,
                        VisitorId = WebSecurity.CurrentUserId,
                        Timestamp = DateTime.Now
                    });
                }
                else
                {
                    visitor.Timestamp = DateTime.Now;
                    db.Visitors.Update(visitor);
                    
                }
                db.SaveChanges();
            }

            Mapper.CreateMap<Models.Profile, ProfileView>();
            var profileView = Mapper.Map<Models.Profile, ProfileView>(profile);

            return View(profileView);
        }


        // GET: /Profile/Edit/5
        public ActionResult Edit(string username = "")
        {
            var profile = db.Profiles.Single(p => p.UserName == username);

            Mapper.CreateMap<Models.Profile, ProfileView>();
            var profileview = Mapper.Map<Models.Profile, ProfileView>(profile);

            return View(profileview);
        }

        // POST: /Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileView profileview)
        {
            Mapper.CreateMap<ProfileView, Models.Profile>()
                .ForMember(src => src.Email, opt => opt.Ignore());

            var profile = Mapper.Map<ProfileView, Models.Profile>(profileview);
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