using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MvcDating.Models;
using WebMatrix.WebData;

namespace MvcDating.Services
{
    public class VisitorRepository : GenericRepository<Visitor>
    {
        public VisitorRepository(UsersContext context)
            : base(context)
        {
        }

        public IEnumerable<VisitorView> GetMyVisits()
        {
            var visitorView = from visitor in Context.Visitors
                              join profile in Context.Profiles on visitor.VisitorId equals profile.UserId
                              join picture in Context.Pictures on profile.UserId equals picture.UserId
                              where visitor.VisitorId == WebSecurity.CurrentUserId
                              select new VisitorView
                              {
                                  UserId = visitor.UserId,
                                  Timestamp = visitor.Timestamp,
                                  Profile = profile,
                                  Thumb = picture.Thumb
                              };

            return visitorView;
        }
    }
}