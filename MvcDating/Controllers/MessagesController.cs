using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using MvcDating.Models;
using MvcDating.Filters;
using WebMatrix.WebData;

namespace MvcDating.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class MessagesController : Controller
    {
        private UsersContext db = new UsersContext();

        //
        // GET: /Messages/

        public ActionResult Index()
        {

            var conversations = from c in db.Conversations
                                where (c.UserIdFrom == WebSecurity.CurrentUserId || c.UserIdTo == WebSecurity.CurrentUserId)
                                select c;

            var conversationView = conversations.ToList().Select(c => new ConversationView()
            {
                ConversationId = c.ConversationId,
                UserPicture = c.GetUserPicture(c.UserIdFrom == WebSecurity.CurrentUserId ? c.UserIdTo : c.UserIdFrom),
                UserNameWith = c.GetUserName(c.UserIdFrom == WebSecurity.CurrentUserId ? c.UserIdTo : c.UserIdFrom),
                LastMessage = c.GetLastMessage(),
                Timestamp = c.Timestamp
            });

            return View(conversationView.ToList());
        }

        //
        // GET: /Messages/Read/5

        public ActionResult Read(int id = 0)
        {
            Conversation conversation = db.Conversations.Find(id);

            if (conversation == null) return HttpNotFound();

            var messages = from msg in conversation.Messages select msg;

            var messageView = messages.ToList().Select(msg => new MessageView
            {
                UserId = msg.UserId,
                UserName = msg.GetUserName(msg.UserId),
                UserPicture = msg.GetUserPicture(msg.UserId),
                Content = msg.Content,
                Timestamp = msg.Timestamp
            });

            ViewBag.ConversationId = id;
            ViewBag.UserIdWith = conversation.UserIdFrom != WebSecurity.CurrentUserId
                               ? conversation.UserIdFrom
                               : conversation.UserIdTo;

            return View(messageView.ToList());
        }

        //
        // GET: /Messages/Create
        // On: /Profile/{userName}
        public ActionResult Create(int userId = 0)
        {
            var profile = db.Profiles.Find(userId);

            ViewBag.userName = profile.UserName;
            ViewBag.userPicture = profile.GetUserPicture(userId);
            return PartialView(new Message()
            {
                UserId = userId
            });
        }

        public Conversation GetConversation(int userId)
        {
            // See if a conversion already exists
            return (from conv in db.Conversations
                    where ((conv.UserIdTo == userId && conv.UserIdFrom == WebSecurity.CurrentUserId)
                        || (conv.UserIdTo == WebSecurity.CurrentUserId && conv.UserIdFrom == userId))
                    select conv).SingleOrDefault();
        }

        //
        // POST: /Messages/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Message message)
        {
            message.Timestamp = DateTime.Now;

            if (ModelState.IsValid)
            {
                // See if a conversion already exists
                var conversation = GetConversation(message.UserId);

                if (conversation == null)
                {
                    // Create new
                    var newConversation = new Conversation()
                    {
                        UserIdFrom = WebSecurity.CurrentUserId,
                        UserIdTo = message.UserId,
                        Timestamp = DateTime.Now,
                        Messages = new List<Message> { message }
                    };
                    message.ConversationId = newConversation.ConversationId;
                    db.Conversations.Add(newConversation);
                }
                else
                {
                    message.UserId = WebSecurity.CurrentUserId;
                    conversation.Messages.Add(message);
                    conversation.Timestamp = DateTime.Now;
                }

                // Set this user as online
                Helpers.User.SetOnline();

                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Read", routeValues: new { id = message.ConversationId });
            }

            return View();
        }

        //
        // GET: /Messages/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Conversation conversation = db.Conversations.Find(id);
            if (conversation == null)
            {
                return HttpNotFound();
            }
            return View(conversation);
        }

        //
        // POST: /Messages/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Conversation conversation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conversation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(conversation);
        }

        //
        // GET: /Messages/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Conversation conversation = db.Conversations.Find(id);
            if (conversation == null)
            {
                return HttpNotFound();
            }
            return View(conversation);
        }

        //
        // POST: /Messages/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conversation conversation = db.Conversations.Find(id);
            db.Conversations.Remove(conversation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}