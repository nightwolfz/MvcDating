﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Domain.Filters;
using Domain.Models;
using MvcDating.Models;
using MvcDating.Services;
using WebMatrix.WebData;

namespace MvcDating.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class MessagesController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        //
        // GET: /Messages/

        public ActionResult Index()
        {
            var conversationView = db.Conversations.GetConversationsView(WebSecurity.CurrentUserId);

            return View(conversationView);
        }

        //
        // GET: /Messages/Read/5

        public ActionResult Read(int id = 0)
        {
            Conversation conversation = db.Conversations.Find(id);

            if (conversation == null) return HttpNotFound();

            var messageView = db.Messages.GetMessagesView(conversation);

            ViewBag.ConversationId = id;
            ViewBag.UserIdWith = conversation.UserIdFrom != WebSecurity.CurrentUserId
                               ? conversation.UserIdFrom
                               : conversation.UserIdTo;

            return View(messageView);
        }

        //
        // GET: /Messages/Create
        // On: /Profile/{userName}
        public ActionResult Create(int userId = 0)
        {
            var profile = db.Profiles.Single(p => p.UserId == userId);

            ViewBag.userName = profile.UserName;
            ViewBag.userPicture = db.Profiles.GetUserPicture(userId);
            return PartialView(new Message
            {
                UserId = userId
            });
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
                var conversation = db.Conversations.GetConversation(message.UserId);

                if (conversation == null)
                {
                    // Create new
                    var newConversation = new Conversation
                    {
                        UserIdFrom = WebSecurity.CurrentUserId,
                        UserIdTo = message.UserId,
                        Timestamp = DateTime.Now,
                        Messages = new List<Message> {message}
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
            }
            return RedirectToAction("Read", routeValues: new {id = message.ConversationId});
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
                db.Conversations.Update(conversation);
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
            db.Conversations.Delete(conversation);
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