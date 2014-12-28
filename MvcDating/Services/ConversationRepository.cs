using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Sql;
using System.Linq;
using System.Security.Cryptography;
using Domain.Models;
using Microsoft.Data.OData.Query.SemanticAst;
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


        public Message GetLastMessage(Conversation conversation, int otherUserId)
        {
            //var lastDate = Context.Messages.Where(m => m.ConversationId == conversationId).Max(m => m.Timestamp);
            //var lastMessage = Context.Messages.Where(m => m.ConversationId == conversationId && m.Timestamp == lastDate).ToArray().Last();
            return (from msg in conversation.Messages where msg.UserId == otherUserId select msg).ToArray().Last();
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
            var conversations = (from conv in Context.Conversations 
                    where (conv.UserIdFrom == userId || conv.UserIdTo == userId)
                    select new
                    {
                        otherUserId = (conv.UserIdFrom == userId ? conv.UserIdTo : conv.UserIdFrom),
                        conversation = conv
                    }).AsEnumerable();

            return conversations.Select(conv => new ConversationView
                    {
                        ConversationId = conv.conversation.ConversationId,
                        UserPicture = GetUserPicture(conv.otherUserId),
                        UserNameWith = GetUserName(conv.otherUserId),
                        LastMessage = GetLastMessage(conv.conversation, conv.otherUserId),
                        Timestamp = conv.conversation.Timestamp
                    });
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