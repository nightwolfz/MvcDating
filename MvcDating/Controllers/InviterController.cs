using System.Collections.Generic;
using System.Web.Mvc;
using Google.Contacts;
using Google.GData.Client;
using Google.GData.Extensions;
using MvcDating.Filters;
using MvcDating.Models;
using MvcDating.Services;

namespace MvcDating.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class InviterController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        // GET: /Inviter/
        public ActionResult Index()
        {
            ViewBag.ServiceName = "Google Mail";

            var inviterLoginView = new InviterLoginView();

            return View(inviterLoginView);
        }

        [HttpPost]
        public ActionResult Index(InviterLoginView inviterLoginView)
        {
            ViewBag.ServiceName = "Google Mail";

            var contacts = InviterDotNet.GetGmailContacts("InviterDotNet", inviterLoginView.Email, inviterLoginView.Password);

            ViewBag.Contacts = contacts;

            return View(inviterLoginView);
        }

	}



    class InviterDotNet
    {
        public static List<ContactsView> GetGmailContacts(string App_Name, string Uname, string UPassword)
        {
            var contacts = new List<ContactsView>();

            var rs = new RequestSettings(App_Name, Uname, UPassword){ AutoPaging = true };

            var contactsRequest = new ContactsRequest(rs);
            var feed = contactsRequest.GetContacts();

            foreach (Contact contact in feed.Entries)
            {
                var fetchedContact = new ContactsView { Name = contact.Name.FullName };

                foreach (EMail email in contact.Emails)
                {
                    if (email.Address == null) continue;
                    fetchedContact.Email = email.Address;
                }
                contacts.Add(fetchedContact);
            }
            return contacts;
        }
    }

}