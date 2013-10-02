namespace MvcDating.Migrations
{
    using MvcDating.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcDating.Models.UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


        protected override void Seed(MvcDating.Models.UsersContext context)
        {
            // Lets create two users
            string _username = "nightwolfz";
            string _betaname = "xerios";
            string _password = "aaaaaa";


            // Must be same as in Filters/InitializeSimpleMembershipAttribute.cs
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "UserId", "UserName", autoCreateTables: true);

            // Add users and roles
            if (!Roles.RoleExists("Administrator")) Roles.CreateRole("Administrator");
            if (!WebSecurity.UserExists(_username))
            {
                WebSecurity.CreateUserAndAccount(_username, _password/*, propertyValues: new {Email = "test@test.com"}*/);
                WebSecurity.CreateUserAndAccount(_betaname, _password/*, propertyValues: new {Email = "test@test.com"}*/);
            }
            if (!Roles.GetRolesForUser(_username).Contains("Administrator")) Roles.AddUsersToRoles(new[] { _username, _betaname }, new[] { "Administrator" });

            int UserIdToAdd = WebSecurity.GetUserId(_username);
            int BetaIdToAdd = WebSecurity.GetUserId(_betaname);

            // Initial data seed
            context.Profiles.AddOrUpdate(p => p.UserId, new Profile { 
                UserId = UserIdToAdd, 
                UserName = _username,
                Gender = "m",
                Birthday = DateTime.ParseExact("27/03/1989 00:00:01 AM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                UpdatedDate = DateTime.Now,
                LocationCountry = "BE",
                LocationCity = "Brussels",
            });
            context.Profiles.AddOrUpdate(p => p.UserId, new Profile { 
                UserId = BetaIdToAdd, 
                UserName = _betaname, 
                Gender = "f" ,
                Birthday = DateTime.ParseExact("27/03/1989 00:00:01 AM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                UpdatedDate = DateTime.Now,
                LocationCountry = "BE",
                LocationCity = "Brussels",
            });


            context.Pictures.AddOrUpdate(p => p.PictureId, new[]{
                    new Picture
                    {
                        UserId = UserIdToAdd,
                        Src = "x_test.jpg",
                        Thumb = "s_test.jpg",
                        IsAvatar = true,
                        UploadedDate = DateTime.Now,
                        Comments = new List<Comment>
                        {
                            new Comment() { UserId = UserIdToAdd, Content = "First!" },
                            new Comment() { UserId = UserIdToAdd, Content = "Second!" },
                        }
                    },
                    new Picture
                    {
                        UserId = BetaIdToAdd,
                        Src = "x_test.jpg",
                        Thumb = "s_test.jpg",
                        IsAvatar = true,
                        UploadedDate = DateTime.Now,
                        Comments = new List<Comment>{}
                    },
            });

            context.Conversations.AddOrUpdate(p => p.ConversationId, new Conversation
            {
                UserIdFrom = UserIdToAdd,
                UserIdTo = BetaIdToAdd,
                Timestamp = DateTime.Now,
                Messages = new List<Message>
                {
                    new Message(){ Content = "Test content1!",  UserId = UserIdToAdd, Timestamp = DateTime.Now },
                    new Message(){ Content = "Test content2!",  UserId = BetaIdToAdd, Timestamp = DateTime.Now },
                }
            });

            context.SaveChanges();
        }

    }
}
