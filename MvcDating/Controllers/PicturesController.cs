using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDating.Models;
using WebMatrix.WebData;
using MvcDating.Filters;

namespace MvcDating.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class PicturesController : Controller
    {
        private UsersContext db = new UsersContext();


        // GET: /Pictures/
        public ActionResult Index()
        {
            var pictures = (from p in db.Pictures where p.UserId == WebSecurity.CurrentUserId orderby p.IsAvatar descending select p).ToList();

            return View(pictures);
        }

        // GET: /Pictures/Upload
        public ActionResult Upload()
        {
            ViewData["UserId"] = WebSecurity.CurrentUserId;
            return View("/Views/Pictures/Index.cshtml");
        }

        // POST: /Pictures/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(Picture picture, HttpPostedFileBase imageData)
        {
            if (ModelState.IsValid)
            {
                if (imageData.ContentLength > 0)
                {
                    var renamedFileName = Helpers.Upload.UploadAndRename(imageData);

                    picture.UserId = WebSecurity.CurrentUserId;
                    picture.UploadedDate = DateTime.Now;
                    picture.Comments = new List<Comment>();
                    picture.Thumb = renamedFileName[0];
                    picture.Src = renamedFileName[1];
                    picture.IsAvatar = !db.Pictures.Any(p => p.IsAvatar);

                    db.Pictures.Add(picture);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View("/Views/Pictures/Index.cshtml");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}