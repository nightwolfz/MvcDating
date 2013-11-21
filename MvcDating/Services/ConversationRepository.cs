using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using MvcDating.Models;
using WebMatrix.WebData;

namespace MvcDating.Services
{
    public class ConversationRepository : GenericRepository<Conversation>
    {
        public ConversationRepository(UsersContext context)
            : base(context)
        {
        }


        public Message GetLastMessage(int conversationId)
        {
            var lastDate = Context.Messages.Where(m => m.ConversationId == conversationId).Max(m => m.Timestamp);
            var lastMessage = Context.Messages.Where(m => m.ConversationId == conversationId && m.Timestamp == lastDate).ToArray().Last();
            return lastMessage;
        }

        public string GetUserPicture(int userId)
        {
            var pictureQuery = Context.Pictures.SingleOrDefault(p => p.IsAvatar && p.UserId == userId);
            return pictureQuery == null ? "default.png" : pictureQuery.Thumb;
        }

        public string GetUserName(int userId)
        {
            return Context.Profiles.Single(p => p.UserId == userId).UserName;
        }

        public IEnumerable<ConversationView> GetConversationsView(int userId)
        {
            var conversations = from c in Context.Conversations
                                where (c.UserIdFrom == userId || c.UserIdTo == userId)
                                select c;

            var conversationView = conversations.AsEnumerable().Select(c => new ConversationView
            {
                ConversationId = c.ConversationId,
                UserPicture = GetUserPicture(c.UserIdFrom == userId ? c.UserIdTo : c.UserIdFrom),
                UserNameWith = GetUserName(c.UserIdFrom == userId ? c.UserIdTo : c.UserIdFrom),
                LastMessage = GetLastMessage(c.ConversationId),
                Timestamp = c.Timestamp
            });
            return conversationView;
        }

        /// <summary>
        /// Check if a conversation exists, if it does, return it
        /// </summary>
        public Conversation GetConversation(int userId)
        {
            return (from conv in Context.Conversations
                    where ((conv.UserIdTo == userId && conv.UserIdFrom == WebSecurity.CurrentUserId)
                        || (conv.UserIdTo == WebSecurity.CurrentUserId && conv.UserIdFrom == userId))
                    select conv).SingleOrDefault();
        }

    }
}