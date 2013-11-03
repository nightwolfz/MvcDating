using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<ConversationView> GetConversationsView(int userId)
        {
            var conversations = from c in Context.Conversations
                                where (c.UserIdFrom == userId || c.UserIdTo == userId)
                                select c;

            var conversationView = conversations.ToList().Select(c => new ConversationView
            {
                ConversationId = c.ConversationId,
                UserPicture = c.GetUserPicture(c.UserIdFrom == userId ? c.UserIdTo : c.UserIdFrom),
                UserNameWith = c.GetUserName(c.UserIdFrom == userId ? c.UserIdTo : c.UserIdFrom),
                LastMessage = c.GetLastMessage(),
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