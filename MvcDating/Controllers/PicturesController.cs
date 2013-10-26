using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDating.Models;
using WebMatrix.WebData;
using MvcDating.Filters;
using System.IO;
using System.Diagnostics;
using MvcDating.Helpers;

namespace MvcDating.Controllers
{
    [InitializeSimpleMembership]
    public class PicturesController : Controller
    {
        private UsersContext db = new UsersContext();

        //
        // GET: /Pictures/

        public ActionResult Index()
        {
            var pictures = (from p in db.Pictures where p.UserId == WebSecurity.CurrentUserId orderby p.IsAvatar descending select p).ToList();

            return View(pictures);
        }

        //
        // GET: /Pictures/Details/5

        public ActionResult Details(int id = 0)
        {
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        //
        // GET: /Pictures/Create

        public ActionResult Create()
        {
            ViewData["UserId"] = WebSecurity.CurrentUserId;
            return View();
        }

        //
        // POST: /Pictures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Picture picture, HttpPostedFileBase imageData)
        {
            if (ModelState.IsValid)
            {
                if (imageData.ContentLength > 0)
                {
                    var helper = new HelperFunctions();
                    var renamedFileName = helper.UploadAndRename(imageData);

                    picture.UserId = WebSecurity.CurrentUserId;
                    picture.UploadedDate = DateTime.Now;
                    picture.Comments = new List<Comment>();
                    picture.Src = renamedFileName;
                    picture.Thumb = renamedFileName.Replace("x_","s_");
                    picture.IsAvatar = !db.Pictures.Any(p => p.IsAvatar);

                    /*var fileName = Path.GetFileName(imageData.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                    imageData.SaveAs(path);*/

                    db.Pictures.Add(picture);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(picture);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}