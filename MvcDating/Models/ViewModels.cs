using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MvcDating.Models
{
    public static class ProfileItems
    {
        public static List<string> Gender = new List<string>()
        {
            "Male", "Female"
        };

        public static List<string> Situation = new List<string>()
        {
            "Single", "Seeing someone", "Married"
        };

        public static List<string> Orientation = new List<string>()
        {
            "Straight", "Gay", "Bisexual"
        };


    }

    public class ProfileView
    {
        public int UserId { get; set; }

        [HiddenInput]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [DisplayName("Gender")]
        public int Gender { get; set; }
        public IEnumerable<SelectListItem> GenderItems
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem { Text = ProfileItems.Gender[0], Value = "0" },
                    new SelectListItem { Text = ProfileItems.Gender[1], Value = "1" }
                };
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
        public IEnumerable<SelectListItem> SituationItems
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem { Text = ProfileItems.Situation[0], Value = "0" },
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
                    new SelectListItem { Text = ProfileItems.Orientation[0], Value = "0" },
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

        [DisplayName("Updated on")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set; }

        public string GetTimeAgo()
        {
            return Helpers.Time.GetTimeAgo(UpdatedDate);
        }

        public bool GetOnlineStatus()
        {
            return DateTime.Now.Subtract(UpdatedDate).TotalMinutes < 15;
        }

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

        [DisplayName("Posted on")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime Timestamp { get; set; }
    }


    public class ConversationView
    {
        [HiddenInput(DisplayValue = false)]
        public int ConversationId { get; set; }

        [DataType(DataType.ImageUrl)]
        public string UserPicture { get; set; }

        public string UserNameWith { get; set; }// one-to-many

        public Message LastMessage { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime Timestamp { get; set; }

        public string GetTimeAgo()
        {
            return Helpers.Time.GetTimeAgo(Timestamp);
        }

    }

    public class PictureDeleteView
    {
        [HiddenInput(DisplayValue = false)]
        public int PictureId { get; set; }
    }

    public class FeaturedView
    {
        public string UserName { get; set; }
        public string Thumb { get; set; }
    }

    public class VisitorView
    {
        [Key]
        public int UserId { get; set; } // my id
        public Profile Profile { get; set; }
        public string Thumb { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime Timestamp { get; set; }

        public string GetTimeAgo()
        {
            return Helpers.Time.GetTimeAgo(Timestamp);
        }
    }

    public class SearchView
    {
        public SearchBoxView SearchBox { get; set; }
        public IEnumerable<SearchResultView> SearchResults { get; set; }
    }

    public class SearchBoxView
    {
        public SearchBoxView()
        {
            Gender = new List<int>{0,1};
        }

        [DisplayName("From")]
        public int AgeFrom { get; set; }

        [DisplayName("to")]
        public int AgeTo { get; set; }

        [DisplayName("Living in")]
        public string LocationCity { get; set; }

        [DisplayName("Who are")]
        public IList<int> Gender { get; set; }
        public IEnumerable<string> GenderItems = ProfileItems.Gender;

        [DisplayName("Current situation")]
        public int Situation { get; set; }
        public IEnumerable<string> SituationItems = ProfileItems.Situation;

        [DisplayName("Sexual orientation")]
        public int Orientation { get; set; }
        public IEnumerable<string> OrientationItems = ProfileItems.Orientation;
    }

    public class SearchResultView
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Birthday { get; set; }
        public string Age
        {
            get
            {
                return Helpers.Time.GetAge(Birthday);
            }
        }
        public int Gender { get; set; }
        public int Orientation { get; set; }
        public int Situation { get; set; }
        public string LocationCountry { get; set; }
        public string LocationCity { get; set; }
        public string Thumb { get; set; }
        public int PictureCount { get; set; }
    }
}