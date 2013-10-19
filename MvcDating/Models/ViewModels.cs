using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcDating.Models
{
    public static class ProfileItems
    {
        public static Dictionary<int, string> Gender
        {
            get
            {
                return new Dictionary<int, string>(){ {0, "Male"}, {1, "Female"} };
            }
        }
        public static Dictionary<int, string> Situation
        {
            get
            {
                return new Dictionary<int, string>() { { 0, "Single" }, { 1, "Seeing someone" }, { 2, "Married" } };
            }
        }
        public static Dictionary<int, string> Orientation
        {
            get
            {
                return new Dictionary<int, string>() { { 0, "Straight" }, { 1, "Gay" }, { 2, "Bisexual" } };
            }
        }

    }

    public class ProfileView
    {
        public int UserId { get; set; }

        [Editable(false)]
        [DisplayName("Username")]
        public string UserName { get; set; }

        public int Gender { get; set; }

        public IEnumerable<SelectListItem> GenderItems
        {
            get
            {
                var list = new List<SelectListItem>()
                {
                    new SelectListItem { Text = ProfileItems.Gender[0], Value = "0", Selected = true },
                    new SelectListItem { Text = ProfileItems.Gender[1], Value = "1" }
                };
                
                return list;
            }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Birthday must be set")]
        public DateTime Birthday { get; set; }

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
            set { }
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem { Text = ProfileItems.Situation[0], Value = "0", Selected = true },
                    new SelectListItem { Text = ProfileItems.Situation[1], Value = "1" },
                    new SelectListItem { Text = ProfileItems.Situation[2], Value = "2" }
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
                    new SelectListItem { Text = ProfileItems.Orientation[0], Value = "0", Selected = true },
                    new SelectListItem { Text = ProfileItems.Orientation[1], Value = "1" },
                    new SelectListItem { Text = ProfileItems.Orientation[2], Value = "2" }
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

        public string LastMessage { get; set; }

        [Timestamp]
        public DateTime? Timestamp { get; set; }
    }

    public class PictureDeleteView
    {
        [HiddenInput(DisplayValue = false)]
        public int PictureId { get; set; }
    }
}