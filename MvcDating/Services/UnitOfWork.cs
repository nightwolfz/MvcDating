using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDating.Models;

namespace MvcDating.Services
{
    public class UnitOfWork : IDisposable
    {
        UsersContext _context = new UsersContext();
        
        ProfileRepository _profile;
        public ProfileRepository Profiles
        {
            get { return _profile ?? (_profile = new ProfileRepository(_context)); }
        }

        PictureRepository _picture;
        public PictureRepository Pictures
        {
            get { return _picture ?? (_picture = new PictureRepository(_context)); }
        }

        ConversationRepository _conversation;
        public ConversationRepository Conversations
        {
            get { return _conversation ?? (_conversation = new ConversationRepository(_context)); }
        }

        MessageRepository _message;
        public MessageRepository Messages
        {
            get { return _message ?? (_message = new MessageRepository(_context)); }
        }

        VisitorRepository _visitor;
        public VisitorRepository Visitors
        {
            get { return _visitor ?? (_visitor = new VisitorRepository(_context)); }
        }

        WinkRepository _wink;
        public WinkRepository Winks
        {
            get { return _wink ?? (_wink = new WinkRepository(_context)); }
        }



        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        // Release ressources
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing) _context.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}