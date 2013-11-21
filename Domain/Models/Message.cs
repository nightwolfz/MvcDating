using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain.Models
{
    public class Message
    {
        private UsersContext db = new UsersContext();

        [Key]
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Must be between {2} and {1} characters long.")]
        public string Content { get; set; }

        public DateTime Timestamp { get; set; }
        public virtual Conversation Conversation { get; set; } // many-to-one
    }
}