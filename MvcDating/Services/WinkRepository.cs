using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDating.Models;
using WebMatrix.WebData;

namespace MvcDating.Services
{
    public class WinkRepository : GenericRepository<Wink>
    {
        public WinkRepository(UsersContext context)
            : base(context)
        {
        }

        public IEnumerable<VisitorView> GetMyWinks()
        {
            var winkView = from wink in Context.Winks
                            join profile in Context.Profiles on wink.UserId equals profile.UserId
                            join picture in Context.Pictures on profile.UserId equals picture.UserId
                            where wink.UserId == WebSecurity.CurrentUserId
                            select new VisitorView
                            {
                                UserId = wink.UserId,
                                Timestamp = wink.Timestamp,
                                Profile = profile,
                                Thumb = picture.Thumb
                            };

            return winkView;
        }

        public void AddWink(int userId)
        {
            if (userId != WebSecurity.CurrentUserId)
            {
                var wink = Context.Winks.SingleOrDefault(dto => dto.UserId == WebSecurity.CurrentUserId);

                if (wink == null)
                {
                    Context.Winks.Add(new Wink
                    {
                        UserId = userId,
                        WinkerId = WebSecurity.CurrentUserId,
                        Timestamp = DateTime.Now
                    });
                }
                else
                {
                    wink.Timestamp = DateTime.Now;
                    Update(wink);
                }
                Context.SaveChanges();
            }
        }

    }
}