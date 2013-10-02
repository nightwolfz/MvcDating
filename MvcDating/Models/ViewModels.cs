using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcDating.Models
{
    public class ProfileView
    {
        [Key]
        public int UserId { get; set; }

        [Editable(false)]
        [DisplayName("Username")]
        public string UserName { get; set; }

        public IEnumerable<SelectListItem> GenderItems
        {
            get
            {
                var list = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Male", Value = "0", Selected = true },
                    new SelectListItem { Text = "Female", Value = "1" }
                };
                
                return list;
            }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Birthday must be set")]
        public DateTime? Birthday { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Country")]
        public string LocationCountry { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("City")]
        public string LocationCity { get; set; }

        [DisplayName("Relationship status")]
        public int Situation { get; set; }

        [Required(ErrorMessage = "Your relationship status must be set")]
        public IEnumerable<SelectListItem> SituationItems
        {
            set { value = value; }
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Single", Value = "0", Selected = true },
                    new SelectListItem { Text = "Seeing someone", Value = "1" },
                    new SelectListItem { Text = "Married", Value = "2" }
                };
            }
        }

        public int Orientation { get; set; }
        public IEnumerable<SelectListItem> OrientationItems
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Straight", Value = "0", Selected = true },
                    new SelectListItem { Text = "Gay", Value = "1" },
                    new SelectListItem { Text = "Bisexual", Value = "2" }
                };
            }
        }

        [DataType(DataType.MultilineText)]
        [DisplayName("My summary")]
        public string Summary { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("I'm good at")]
        public string GoodAt { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Message me if")]
        public string MessageIf { get; set; }

        [Timestamp]
        [DisplayName("Updated on")]
        public DateTime? UpdatedDate { get; set; }

        public virtual IList<Picture> Pictures { get; set; }
    }

    public class MessageView
    {
        [HiddenInput(DisplayValue = false)]
        public int ConversationId { get; set; }

        public int UserId { get; set; }
        [DisplayName("Username")]
        public string UserName { get; set; }

        [DataType(DataType.ImageUrl)]
        [DisplayName("Picture")]
        public string UserPicture { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        [Timestamp]
        [DisplayName("Posted on")]
        public DateTime? Timestamp { get; set; }
    }


    public class ConversationView
    {
        [HiddenInput(DisplayValue = false)]
        public int ConversationId { get; set; }

        [DataType(DataType.ImageUrl)]
        public string UserPicture { get; set; }

        public string UserNameWith { get; set; }// one-to-many

        [Timestamp]
        public DateTime? Timestamp { get; set; }
    }

    public class PictureDeleteView
    {
        [HiddenInput(DisplayValue = false)]
        public int PictureId { get; set; }
    }
}