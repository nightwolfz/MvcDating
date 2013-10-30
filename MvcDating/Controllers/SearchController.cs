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
using Profile = MvcDating.Models.Profile;

namespace MvcDating.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: /Search/
        public ActionResult Index()
        {
            var profiles = db.Profiles.ToList();

            var resultsView = new SearchView
            {
                SearchBox = new SearchBoxView(),
                SearchResults = CreateSearchResultMapping(profiles)
            };

            return View(resultsView);
        }

        [HttpPost]
        public ActionResult Index(SearchBoxView searchBoxView)
        {
            var profiles = (from p in db.Profiles where searchBoxView.Gender.Contains(p.Gender) select p).ToList();

            var resultsView = new SearchView
            {
                SearchBox = searchBoxView,
                SearchResults = CreateSearchResultMapping(profiles)
            };

            return View(resultsView);
        }

        private IEnumerable<SearchResultView> CreateSearchResultMapping(IEnumerable<Profile> profiles)
        {
            Mapper.CreateMap<MvcDating.Models.Profile, SearchResultView>()
                .ForMember(src => src.Thumb, opt => opt.MapFrom(c => c.Pictures.SingleOrDefault(p => p.IsAvatar).Thumb))
                .ForMember(src => src.Thumb, opt => opt.NullSubstitute("default.png"));

            return Mapper.Map<IEnumerable<Profile>, IEnumerable<SearchResultView>>(profiles);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}