using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using AutoMapper;
using MvcDating.Models;

namespace MvcDating.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private UsersContext db = new UsersContext();

        //
        // GET: /Search/

        public ActionResult Index()
        {
            var profiles = db.Profiles.ToList();

            Mapper.CreateMap<MvcDating.Models.Profile, SearchResultView>()
                .ForMember(
                src => src.Thumb, 
                opt => opt.MapFrom(c => c.Pictures.Single(p => p.IsAvatar).Thumb)
            );

            var resultsView = Mapper.Map<IEnumerable<MvcDating.Models.Profile>, IEnumerable<SearchResultView>>(profiles);

            return View(resultsView);
        }

        [HttpPost]
        public ActionResult Index(SearchView searchView)
        {
            if (searchView.Gender == null)
                searchView.Gender = new List<int>(0);

            var profiles = (from p in db.Profiles where searchView.Gender.Contains(p.Gender) select p).ToList();

            Mapper.CreateMap<MvcDating.Models.Profile, SearchResultView>()
                .ForMember(
                src => src.Thumb,
                opt => opt.MapFrom(c => c.Pictures.Single(p => p.IsAvatar).Thumb)
            );

            var resultsView = Mapper.Map<IEnumerable<MvcDating.Models.Profile>, IEnumerable<SearchResultView>>(profiles);

            return View(resultsView);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}