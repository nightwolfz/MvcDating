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
            const string username = "nightwolfz";
            const string betaname = "xerios";
            const string password = "aaaaaa";


            // Must be same as in Filters/InitializeSimpleMembershipAttribute.cs
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "UserId", "UserName", autoCreateTables: true);

            // Add users and roles
            if (!Roles.RoleExists("Administrator")) Roles.CreateRole("Administrator");
            if (!WebSecurity.UserExists(username)) WebSecurity.CreateUserAndAccount(username, password/*, propertyValues: new {Email = "test@test.com"}*/);
            if (!WebSecurity.UserExists(betaname)) WebSecurity.CreateUserAndAccount(betaname, password/*, propertyValues: new {Email = "test@test.com"}*/);
            
            if (!Roles.GetRolesForUser(username).Contains("Administrator")) Roles.AddUsersToRoles(new[] { username, betaname }, new[] { "Administrator" });

            int userIdToAdd = WebSecurity.GetUserId(username);
            int betaIdToAdd = WebSecurity.GetUserId(betaname);

            // Initial data seed
            context.Profiles.AddOrUpdate(p => p.UserId, new Profile { 
                UserId = userIdToAdd, 
                UserName = username,
                Gender = "m",
                Birthday = DateTime.ParseExact("27/03/1989 00:00:01 AM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                UpdatedDate = DateTime.Now,
                LocationCountry = "BE",
                LocationCity = "Brussels",
            });
            context.Profiles.AddOrUpdate(p => p.UserId, new Profile { 
                UserId = betaIdToAdd, 
                UserName = betaname, 
                Gender = "f" ,
                Birthday = DateTime.ParseExact("27/03/1989 00:00:01 AM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                UpdatedDate = DateTime.Now,
                LocationCountry = "BE",
                LocationCity = "Brussels",
            });


            context.Pictures.AddOrUpdate(p => p.PictureId, new[]{
                    new Picture
                    {
                        UserId = userIdToAdd,
                        Src = "x_test.jpg",
                        Thumb = "s_test.jpg",
                        IsAvatar = true,
                        UploadedDate = DateTime.Now,
                        Comments = new List<Comment>
                        {
                            new Comment() { UserId = userIdToAdd, Content = "First!" },
                            new Comment() { UserId = userIdToAdd, Content = "Second!" },
                        }
                    },
                    new Picture
                    {
                        UserId = betaIdToAdd,
                        Src = "x_test.jpg",
                        Thumb = "s_test.jpg",
                        IsAvatar = true,
                        UploadedDate = DateTime.Now,
                        Comments = new List<Comment>{}
                    },
            });

            context.Conversations.AddOrUpdate(p => p.ConversationId, new Conversation
            {
                UserIdFrom = userIdToAdd,
                UserIdTo = betaIdToAdd,
                Timestamp = DateTime.Now,
                Messages = new List<Message>
                {
                    new Message(){ Content = "Test content1!",  UserId = userIdToAdd, Timestamp = DateTime.Now },
                    new Message(){ Content = "Test content2!",  UserId = betaIdToAdd, Timestamp = DateTime.Now },
                }
            });

            context.SaveChanges();
        }

    }
}
