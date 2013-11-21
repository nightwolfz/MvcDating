using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Google.GData.Contacts;
using MvcDating.Models;
using MvcDating.Services;
using WebMatrix.WebData;

namespace MvcDating.Controllers
{
    public class WinkController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        // GET: /Wink/
        public ActionResult Index()
        {
            var winksView = db.Winks.GetMyWinks();

            return View(winksView);
        }

        public ActionResult Add(int userId)
        {
            if (ModelState.IsValid)
            {
                db.Winks.AddWink(userId);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Delete all winks coming from this user
        public ActionResult Delete(int winkerId)
        {
            Wink wink = db.Winks.Single(w => w.WinkerId == winkerId && w.UserId == WebSecurity.CurrentUserId);
            db.Winks.Delete(wink);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
