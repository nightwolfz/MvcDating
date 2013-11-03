using MvcDating.Models;
using System.Linq;
using System.Web.Mvc;
using MvcDating.Services;

namespace MvcDating.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult FeaturedProfiles()
        {
            var featuredUsers = db.Profiles.GetFeatured(5);

            return PartialView(featuredUsers);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
