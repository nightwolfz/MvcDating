using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Domain.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext(): base("DefaultConnection") {}

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Wink> Winks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Profile>().Property(f => f.Birthday).HasColumnType("datetime"); // Before 2000
        }
    }

    public class Profile
    {
        public Profile()
        {
            UpdatedDate = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string LocationCountry { get; set; }
        public string LocationCity { get; set; }
        public int Situation { get; set; }
        public int Orientation { get; set; }
        public string Summary { get; set; }
        public string GoodAt { get; set; }
        public string MessageIf { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual IList<Picture> Pictures { get; set; } // one-to-many

        public void UpdateOnlineStatus()
        {
            UpdatedDate = DateTime.Now;
        }
    }

    public class Picture
    {
        public Picture()
        {
            UploadedDate = DateTime.Now;
        }

        [Key]
        public int PictureId { get; set; }
        public int UserId { get; set; } // one-to-many
        public string Src { get; set; }
        public string Thumb { get; set; }
        public bool IsAvatar { get; set; }
        public DateTime UploadedDate { get; set; }
        public virtual IList<Comment> Comments { get; set; } // one-to-many
    }

    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public int PictureId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public virtual Picture Picture { get; set; } // many-to-one
    }

    public class Visitor
    {
        [Key]
        public int VisitId { get; set; }
        public int VisitorId { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Wink
    {
        [Key]
        public int WinkId { get; set; }
        public int WinkerId { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
