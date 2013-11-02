using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MvcDating.Filters;
using MvcDating.Models;
using WebMatrix.WebData;

namespace MvcDating.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class VisitorsController : Controller
    {
        private UsersContext db = new UsersContext();

        //
        // GET: /Visitors/

        public ActionResult Index()
        {
            var visitors = db.Visitors.Where(v => v.UserId == WebSecurity.CurrentUserId).ToList();

            Mapper.CreateMap<Visitor, VisitorView>()
                .ForMember(src => src.Thumb, opt => opt.MapFrom(c => db.Pictures.FirstOrDefault(p => p.IsAvatar && p.UserId == c.UserId).Thumb))
                .ForMember(src => src.Profile, opt => opt.MapFrom(c => db.Profiles.FirstOrDefault(p => p.UserId == c.UserId)));

            var visitorView = Mapper.Map<IEnumerable<Visitor>, IEnumerable<VisitorView>>(visitors);

            return View(visitorView);
        }


        public ActionResult History()
        {
            var visitorView = from visitor in db.Visitors
                              join profile in db.Profiles on visitor.VisitorId equals profile.UserId
                              join picture in db.Pictures on profile.UserId equals picture.UserId
                              where visitor.VisitorId == WebSecurity.CurrentUserId
                              select new VisitorView
                              {
                                  UserId = visitor.UserId,
                                  Timestamp = visitor.Timestamp,
                                  Profile = profile,
                                  Thumb = picture.Thumb
                              };

            return View(visitorView);
        }
    }
}
