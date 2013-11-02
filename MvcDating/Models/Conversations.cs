using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MvcDating.Models
{
    public class Conversation
    {
        [Key]
        public int ConversationId { get; set; } // one-to-many
        public int UserIdTo { get; set; }
        public int UserIdFrom { get; set; }// one-to-many
        public DateTime Timestamp { get; set; }
        public virtual IList<Message> Messages { get; set; } // one-to-many

        private UsersContext db = new UsersContext();

        public Message GetLastMessage()
        {
            var lastDate = Messages.Where(m => m.ConversationId == ConversationId).Max(m => m.Timestamp);
            var lastMessage = Messages.LastOrDefault(m => m.ConversationId == ConversationId && m.Timestamp == lastDate);
            return lastMessage;
        }

        public string GetUserPicture(int userId)
        {
            var pictureQuery = db.Pictures.SingleOrDefault(p => p.IsAvatar && p.UserId == userId);
            return pictureQuery == null ? "default.png" : pictureQuery.Thumb;
        }

        public string GetUserName(int userId)
        {
            return db.Profiles.Single(p => p.UserId == userId).UserName;
        }
    }

    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual Conversation Conversation { get; set; } // many-to-one

        private UsersContext db = new UsersContext();

        public string GetUserName(int userId)
        {
            return (from profile in db.Profiles where profile.UserId == UserId select profile.UserName).SingleOrDefault();
        }
        public string GetUserPicture(int userId)
        {
            return (from picture in db.Pictures where (picture.IsAvatar && picture.UserId == UserId) select picture.Thumb).SingleOrDefault();
        }
    }
}