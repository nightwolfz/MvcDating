using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDating.Models;

namespace MvcDating.Services
{
    public class MessageRepository : GenericRepository<Message>
    {
        public MessageRepository(UsersContext context)
            : base(context)
        {
        }
    }
}