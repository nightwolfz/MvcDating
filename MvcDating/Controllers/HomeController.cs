using MvcDating.Models;
using System.Linq;
using System.Web.Mvc;

namespace MvcDating.Controllers
{
    public class HomeController : Controller
    {
        private UsersContext db = new UsersContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult FeaturedProfiles()
        {
            var query = from profile in db.Profiles
                        join picture in db.Pictures on profile.UserId equals picture.UserId into ps
                        from picture in ps.DefaultIfEmpty()
                        where picture.IsAvatar
                        select new FeaturedView { UserName = profile.UserName, Thumb = picture.Thumb };

            var featuredUsers = query.Take(5);
            return PartialView(featuredUsers);
        }
    }
}
