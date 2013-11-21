using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MvcDating.Models;
using MvcDating.Services;
using Profile = Domain.Models.Profile;

namespace MvcDating.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        // GET: /Search/
        public ActionResult Index()
        {
            var profiles = db.Profiles.Get();

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
            var min = DateTime.Today.AddYears(-(searchBoxView.AgeTo + 1));
            var max = DateTime.Today.AddYears(-searchBoxView.AgeFrom);

            var profiles = db.Profiles.Get(
                p => searchBoxView.Gender.Contains(p.Gender)
                     && p.Birthday >= min
                     && p.Birthday <= max
            );

            var resultsView = new SearchView
            {
                SearchBox = searchBoxView,
                SearchResults = CreateSearchResultMapping(profiles)
            };

            // Set current user as online
            Helpers.User.SetOnline();

            return View(resultsView);
        }

        private IEnumerable<SearchResultView> CreateSearchResultMapping(IEnumerable<Profile> profiles)
        {
            Mapper.CreateMap<Profile, SearchResultView>()
                .ForMember(src => src.Thumb, opt => opt.MapFrom(c => c.Pictures.SingleOrDefault(p => p.IsAvatar).Thumb))
                .ForMember(src => src.Thumb, opt => opt.NullSubstitute("default.png"))
                .ForMember(src => src.PictureCount, opt => opt.MapFrom(c => c.Pictures.Count()));

            return Mapper.Map<IEnumerable<Profile>, IEnumerable<SearchResultView>>(profiles);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}