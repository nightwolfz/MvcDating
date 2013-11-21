using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain.Models
{
    public class Conversation
    {
        private UsersContext db = new UsersContext();

        [Key]
        public int ConversationId { get; set; } // one-to-many
        public int UserIdTo { get; set; }
        public int UserIdFrom { get; set; }// one-to-many
        public DateTime Timestamp { get; set; }
        public virtual IList<Message> Messages { get; set; } // one-to-many
    }
}