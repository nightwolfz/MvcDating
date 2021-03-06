using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using Domain.Models;
using WebMatrix.WebData;

namespace Domain.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


        protected override void Seed(UsersContext context)
        {
            // Lets create two users
            const string username = "nightwolfz";
            const string betaname = "xerios";
            const string password = "aaaaaa";


            // Must be same as in Filters/InitializeSimpleMembershipAttribute.cs
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Profiles", "UserId", "UserName", autoCreateTables: true);

            // Add users and roles
            if (!Roles.RoleExists("Administrator")) Roles.CreateRole("Administrator");
            if (!WebSecurity.UserExists(username)) WebSecurity.CreateUserAndAccount(username, password, new
            {
                Email = "nightwolfz@gmail.com",
                Gender = 0,
                Birthday = DateTime.ParseExact("27/03/1989 00:00:01 AM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                UpdatedDate = DateTime.Now,
                LocationCountry = "BE",
                LocationCity = "Brussels",
                Situation = 0,
                Orientation = 0,
            });

            if (!WebSecurity.UserExists(betaname)) WebSecurity.CreateUserAndAccount(betaname, password, new
            {
                Email = "xerios@gmail.com",
                Gender = 1,
                Birthday = DateTime.ParseExact("08/04/1991 00:00:01 AM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                UpdatedDate = DateTime.Now,
                LocationCountry = "BE",
                LocationCity = "Brussels",
                Situation = 0,
                Orientation = 0,
            });

            if (!Roles.GetRolesForUser(username).Contains("Administrator")) Roles.AddUsersToRoles(new[]{ username, betaname }, new[] { "Administrator" });

            int userIdToAdd = WebSecurity.GetUserId(username);
            int betaIdToAdd = WebSecurity.GetUserId(betaname);

            // Initial data seed
            context.Pictures.AddOrUpdate(p => p.PictureId, new[]{
                    new Picture
                    {
                        UserId = userIdToAdd,
                        Src = "default.png",
                        Thumb = "default.png",
                        IsAvatar = true,
                        UploadedDate = DateTime.Now,
                        Comments = new List<Comment>
                        {
                            new Comment { UserId = userIdToAdd, Content = "First!" },
                            new Comment { UserId = userIdToAdd, Content = "Second!" },
                        }
                    },
                    new Picture
                    {
                        UserId = betaIdToAdd,
                        Src = "default.png",
                        Thumb = "default.png",
                        IsAvatar = true,
                        UploadedDate = DateTime.Now,
                        Comments = new List<Comment>()
                    }
            });

            context.Conversations.AddOrUpdate(p => p.ConversationId, new Conversation
            {
                UserIdFrom = userIdToAdd,
                UserIdTo = betaIdToAdd,
                Timestamp = DateTime.Now,
                Messages = new List<Message>
                {
                    new Message { Content = "Test content1!",  UserId = userIdToAdd, Timestamp = DateTime.Now },
                    new Message { Content = "Test content2!",  UserId = betaIdToAdd, Timestamp = DateTime.Now },
                }
            });

            context.SaveChanges();
        }

    }
}
