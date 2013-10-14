using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using MvcDating.Models;
using MvcDating.Filters;
using WebMatrix.WebData;

namespace MvcDating.Controllers
{
    [InitializeSimpleMembership]
    public class MessagesController : Controller
    {
        private UsersContext db = new UsersContext();

        //
        // GET: /Messages/

        public ActionResult Index()
        {
            var conversationView = new List<ConversationView> { };
            var conversations = (from convo in db.Conversations
                                                where (convo.UserIdFrom == WebSecurity.CurrentUserId || convo.UserIdTo == WebSecurity.CurrentUserId)
                                                select convo).ToList();

            foreach (Conversation conv in conversations)
            {
                int convoWithId = conv.UserIdFrom == WebSecurity.CurrentUserId ? conv.UserIdTo : conv.UserIdFrom;

                conversationView.Add(new ConversationView()
                {
                    ConversationId = conv.ConversationId,
                    UserPicture = (from picture in db.Pictures where (picture.IsAvatar == true && picture.UserId == convoWithId) select picture.Thumb).SingleOrDefault(),
                    UserNameWith = (from profile in db.Profiles where profile.UserId == convoWithId select profile.UserName).SingleOrDefault(),
                    LastMessage = (from m in conv.Messages orderby m.Timestamp descending select m.Content).FirstOrDefault(),
                    Timestamp = DateTime.Now
                });
            }

            return View(conversationView.ToList());
        }

        //
        // GET: /Messages/Read/5

        public ActionResult Read(int id = 0)
        {
            Conversation conversation = db.Conversations.Find(id);

            if (conversation == null)
            {
                return HttpNotFound();
            }

            var messageView = new List<MessageView> { };

            foreach (Message msg in conversation.Messages)
            {
                messageView.Add(new MessageView{
                    UserId = msg.UserId,
                    UserName = (from profile in db.Profiles where profile.UserId == msg.UserId select profile.UserName).SingleOrDefault(),
                    UserPicture = (from picture in db.Pictures where (picture.IsAvatar == true && picture.UserId == msg.UserId) select picture.Thumb).SingleOrDefault(),
                    Content = msg.Content,
                    Timestamp = msg.Timestamp
                });
            }

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
            ViewBag.userPicture = (from p in db.Pictures where (p.UserId==userId && p.IsAvatar == true) select p.Thumb).FirstOrDefault();
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