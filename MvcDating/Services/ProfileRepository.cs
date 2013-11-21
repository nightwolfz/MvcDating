using System.Linq;
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
    }
}