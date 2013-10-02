using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcDating.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext(): base("DefaultConnection") {}

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<ProfileView> ProfileViews { get; set; }
    }

    /**
     * Good to know: 
     * IEnumerable only for iterating.
     * ICollection for iterating & modifying.
     * IList for iterating, modifying, sorting etc. Very optimized.
     * ICollection is not that fast or useful.
     */

    public class Profile
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string LocationCountry { get; set; }
        public string LocationCity { get; set; }
        public int Situation { get; set; }
        public int Orientation { get; set; }
        public string Summary { get; set; }
        public string GoodAt { get; set; }
        public string MessageIf { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual IList<Picture> Pictures { get; set; }
    }

    public class Picture
    {
        [Key]
        public int PictureId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; } // one-to-many

        [Editable(false)]
        public string Src { get; set; }

        [Editable(false)]
        public string Thumb { get; set; }

        public bool IsAvatar { get; set; }
        [Timestamp]
        public DateTime? UploadedDate { get; set; }

        public virtual IList<Comment> Comments { get; set; }
    }

    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int PictureId { get; set; }
        public int UserId { get; set; }

        [AllowHtml]
        public string Content { get; set; }
       
        public virtual Picture Picture { get; set; }
    }

    public class Conversation
    {
        [Key]
        public int ConversationId { get; set; } // one-to-many

        [HiddenInput(DisplayValue = false)]
        public int UserIdTo { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserIdFrom { get; set; }// one-to-many

        [Timestamp]
        public DateTime? Timestamp { get; set; }

        public virtual IList<Message> Messages { get; set; }
    }

    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime? Timestamp { get; set; }

        public virtual Conversation Conversation { get; set; }
    }

}
