using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TicketTracer.DTO;
using TicketTracer.Util;

namespace TicketTracer.Models
{
    public class TTRepository
    {
        private static string ConnectionString = ConfigurationManager.AppSettings["ConnectionStringDB"].ToString();
        private static string DB = ConfigurationManager.AppSettings["Database"].ToString();
        private static string UserCollection = "User";
        private static string TicketCollection = "Ticket";

        internal static IEnumerable<User> GetAllUser()
        {
            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            return MongoDBHelper.GetCollection<User>(contextDB, UserCollection, Query.Null, Fields.Null);

        }


        internal static User GetUser(string username)
        {
            IMongoQuery query = Query.EQ("NTLogin", username);

            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            var list = MongoDBHelper.GetCollection<User>(contextDB, UserCollection, query, Fields.Null);
            if (list != null)
                return list.FirstOrDefault();
            else
                return null;

        }

        public static bool GetUserAdmin(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;

            var objUser = GetUser(username);
            return (objUser != null && objUser.IsAdmin);
        }

        public static bool GetHelpDeskUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;

            var objUser = GetUser(username);
            return (objUser != null);
        }

        internal static string InsertUser(User entity)
        {
            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            return MongoDBHelper.Insert<User>(contextDB, UserCollection, entity);

        }

        internal static bool UpdatetUser(string id, User value)
        {
            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);

            IMongoUpdate update = Update
                .Set("Name", value.Name)
                .Set("Surname", value.Surname)
                .Set("NTLogin", value.NTLogin)
                .Set("Email", value.Email)
                .Set("Enabled", value.Enabled)
                .Set("Division", value.Division)
                .Set("IsAdmin", value.IsAdmin);

            return MongoDBHelper.Modify(contextDB, UserCollection, id, update);
        }

        internal static bool DeleteUser(string id)
        {
            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            return MongoDBHelper.Delete(contextDB, UserCollection, id);
        }


        internal static IEnumerable<Ticket> GetAllTicket(string user)
        {
            var objUser = GetUser(user);

            IMongoQuery query;
            if (objUser != null && objUser.IsAdmin)
                query = Query.Null;
            else
            {
                if (objUser == null)
                    query = Query.EQ("SubmittedBy", user);
                else
                    query = Query.EQ("AssignedUser", user);
            }

            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            return MongoDBHelper.GetCollection<Ticket>(contextDB, TicketCollection, query, Fields.Null);

        }

        internal static string InsertTicket(Ticket value)
        {
            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            return MongoDBHelper.Insert<Ticket>(contextDB, TicketCollection, value);
        }

        internal static bool UpdatetTicket(string id, Ticket value)
        {
            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);

            //var listComments = (from comm in value.Comments
            //                    //where string.IsNullOrEmpty(comm.Id)
            //                    select new BsonDocument {
            //                    //{ "Id", comm.Id },
            //                    { "Title", comm.Title },
            //                    { "DateCreation", comm.DateCreation },
            //                    { "InsertBy", comm.InsertBy },
            //                    { "Text", comm.Text }
            //                    }).ToArray();

            IMongoUpdate update = Update
                .Set("AssignedUser", value.AssignedUser)
                .Set("Closed", value.Closed)
                .Set("DateClosed", value.DateClosed)
                .Set("Description", value.Description)
                .Set("Title", value.Title);

            return MongoDBHelper.Modify(contextDB, TicketCollection, id, update);
        }

        internal static bool InsertTicketComment(string idTicket, Comment value, string userName)
        {
            var user = TTRepository.GetUser(userName);
            value.InsertBy = (user == null ? userName : user.FullName());

            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);

            var insComm = new BsonDocument {
                { "Title", value.Title },
                { "DateCreation", value.DateCreation },
                { "InsertBy", value.InsertBy },
                { "Text", value.Text }
            };

            IMongoUpdate update = Update.Push("Comments", insComm);

            return MongoDBHelper.Modify(contextDB, TicketCollection, idTicket, update);

        }




        internal static IEnumerable<Comment> GetAllTicketComments(string idTicket)
        {

            IMongoQuery query = Query.EQ("_id", new BsonObjectId(new ObjectId(idTicket)));


            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            return MongoDBHelper.GetCollection<Ticket>(contextDB, TicketCollection, query, Fields.Include("Comments")).FirstOrDefault().Comments;
        }

        internal static bool DeleteTicket(string id)
        {
            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            return MongoDBHelper.Delete(contextDB, TicketCollection, id);
        }
    }
}