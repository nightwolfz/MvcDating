using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
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
                int ConvoWithId = conv.UserIdFrom == WebSecurity.CurrentUserId ? conv.UserIdTo : conv.UserIdFrom;

                conversationView.Add(new ConversationView()
                {
                    ConversationId = conv.ConversationId,
                    UserPicture = (from picture in db.Pictures where (picture.IsAvatar == true && picture.UserId == ConvoWithId) select picture.Thumb).SingleOrDefault(),
                    UserNameWith = (from profile in db.Profiles where profile.UserId == ConvoWithId select profile.UserName).SingleOrDefault(),
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

            return View(messageView.ToList());
        }

        //
        // GET: /Messages/Create
        public ActionResult Create(int UserIdTo = 0)
        {
            ViewBag.UserIdTo = UserIdTo;
            return View();
        }


        //
        // POST: /Messages/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Message message)
        {
            message.UserId = WebSecurity.CurrentUserId;
            message.Timestamp = DateTime.Now;

            if (ModelState.IsValid)
            {
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