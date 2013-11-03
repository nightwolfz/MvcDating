using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MvcDating.Filters;
using MvcDating.Models;
using MvcDating.Services;
using WebMatrix.WebData;

namespace MvcDating.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class VisitorsController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        //
        // GET: /Visitors/

        public ActionResult Index()
        {
            var visitors = db.Visitors.Get(v => v.UserId == WebSecurity.CurrentUserId).ToList();

            Mapper.CreateMap<Visitor, VisitorView>()
                .ForMember(src => src.Thumb, opt => opt.MapFrom(c => db.Pictures.Single(p => p.IsAvatar && p.UserId == c.UserId).Thumb))
                .ForMember(src => src.Profile, opt => opt.MapFrom(c => db.Pictures.Single(p => p.UserId == c.UserId)));

            var visitorView = Mapper.Map<IEnumerable<Visitor>, IEnumerable<VisitorView>>(visitors);

            return View(visitorView);
        }


        public ActionResult History()
        {
            var visitorView = db.Visitors.GetMyVisits();

            return View(visitorView);
        }
    }
}
