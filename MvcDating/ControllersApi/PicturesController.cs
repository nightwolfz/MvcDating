﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MvcDating.Models;
using MvcDating.Filters;
using WebMatrix.WebData;
using System.IO;
using System.Web.Hosting;
using System.Text;

namespace MvcDating.ControllersApi
{
    [InitializeSimpleMembership]
    public class PicturesController : ApiController
    {
        private UsersContext db = new UsersContext();

        // PUT api/Pictures/5
        public HttpResponseMessage PutPicture(int id)
        {
            // Sets the default picture flag
            List<Picture> pictures = (from p in db.Pictures where (p.UserId == WebSecurity.CurrentUserId) select p).ToList();
            foreach (Picture pic in pictures)
            {
                pic.IsAvatar = (pic.PictureId == id) ? true : false;
            }
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/Pictures/5
        public HttpResponseMessage DeletePicture(int id)
        {
            Picture picture = db.Pictures.Single(p => p.PictureId == id && p.UserId == WebSecurity.CurrentUserId);
            if (picture == null) return Request.CreateResponse(HttpStatusCode.NotFound);
            db.Pictures.Remove(picture);
            db.SaveChanges();

            System.IO.File.Delete(Path.Combine(HostingEnvironment.MapPath("~/Uploads"), picture.Src));
            System.IO.File.Delete(Path.Combine(HostingEnvironment.MapPath("~/Uploads"), picture.Thumb));

            return Request.CreateResponse(HttpStatusCode.OK, picture);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}