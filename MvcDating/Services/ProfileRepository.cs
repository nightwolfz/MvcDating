using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Domain.Models;
using MvcDating.Models;

namespace MvcDating.Services
{
    public class ProfileRepository : GenericRepository<Profile>
    {
        public ProfileRepository(UsersContext context)
            : base(context)
        {
        }

        public virtual IQueryable<FeaturedView> GetFeatured(int number = 5)
        {
            var query = from profile in Context.Profiles
                        join picture in Context.Pictures
                        on profile.UserId equals picture.UserId into ps
                        from picture in ps.DefaultIfEmpty()
                        where picture.IsAvatar
                        select new FeaturedView { UserName = profile.UserName, Thumb = picture.Thumb };
            return query.Take(number);
        }

        public string GetUserPicture(int userId)
        {
            var pictureQuery = Context.Pictures.SingleOrDefault(p => p.IsAvatar && p.UserId == userId);
            return pictureQuery == null ? "default.png" : pictureQuery.Thumb;
        }


        public IEnumerable<SelectListItem> GetCountries(string currentCountryTwoLetterISO = "")
        {
            var countryNames = new List<SelectListItem>();

            //To get the Country Names from the CultureInfo installed in windows
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                var country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);
                countryNames.Add(new SelectListItem()
                {
                    Text = country.DisplayName, 
                    Value = country.TwoLetterISORegionName,
                    Selected = (currentCountryTwoLetterISO == country.TwoLetterISORegionName)
                });
            }

            //Assigning all Country names to IEnumerable
            return countryNames.GroupBy(x => x.Text).Select(x => x.FirstOrDefault()).ToList().OrderBy(x => x.Text);
        }
    }
}