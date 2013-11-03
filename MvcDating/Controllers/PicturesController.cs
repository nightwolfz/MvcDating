using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDating.Models;
using MvcDating.Services;
using WebMatrix.WebData;
using MvcDating.Filters;

namespace MvcDating.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class PicturesController : Controller
    {
        private UnitOfWork db = new UnitOfWork();


        // GET: /Pictures/
        public ActionResult Index()
        {
            var pictures = db.Pictures.Get(p => p.UserId == WebSecurity.CurrentUserId, q => q.OrderBy(d => d.IsAvatar));
            return View(pictures);
        }

        // GET: /Pictures/Upload
        public ActionResult Upload()
        {
            ViewData["UserId"] = WebSecurity.CurrentUserId;
            return View("Index");
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
                    picture.IsAvatar = !db.Pictures.Get(p => p.IsAvatar).Any();

                    db.Pictures.Add(picture);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}