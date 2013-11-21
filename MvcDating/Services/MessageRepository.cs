using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Models;
using MvcDating.Models;

namespace MvcDating.Services
{
    public class MessageRepository : GenericRepository<Message>
    {
        public MessageRepository(UsersContext context)
            : base(context)
        {
        }

        public IEnumerable<MessageView> GetMessagesView(Conversation conversation)
        {
            return (from msg in conversation.Messages
             select new MessageView
             {
                 UserId = msg.UserId,
                 UserName = GetUserName(msg.UserId),
                 UserPicture = GetUserPicture(msg.UserId),
                 Content = msg.Content,
                 Timestamp = msg.Timestamp
             }).ToList();
        }

        public string GetUserName(int userId)
        {
            return (from profile in Context.Profiles where profile.UserId == userId select profile.UserName).SingleOrDefault();
        }
        public string GetUserPicture(int userId)
        {
            return (from picture in Context.Pictures where (picture.IsAvatar && picture.UserId == userId) select picture.Thumb).SingleOrDefault();
        }

    }
}