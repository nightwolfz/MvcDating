using Domain.Models;
using MvcDating.Models;

namespace MvcDating.Services
{
    public class PictureRepository : GenericRepository<Picture>
    {
        public PictureRepository(UsersContext context)
            : base(context)
        {
        }

    }
}