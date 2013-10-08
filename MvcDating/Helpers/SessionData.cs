using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDating.Models;

namespace MvcDating.Helpers
{
    public static class Session
    {
        public static Profile Profile
        {
            get
            {
                if (HttpContext.Current.Session.Contents["Profile"] != null)
                {
                    return HttpContext.Current.Session.Contents["Profile"] as Profile;
                }
                else
                {
                    using (var db = new UsersContext())
                    {
                        return db.Profiles.FirstOrDefault(x => x.UserName == HttpContext.Current.User.Identity.Name);
                    }
                }
            }
            set { HttpContext.Current.Session.Contents["Profile"] = value; }
        }
    }
}