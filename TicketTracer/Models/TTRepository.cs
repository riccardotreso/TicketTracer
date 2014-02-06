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

        internal static IEnumerable<User> GetAllUser(bool isHelpDesk)
        {
            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            IMongoQuery query;
            if (isHelpDesk)
                query = Query.EQ("IsHelpDesk", true);
            else
                query = Query.Null;

            return MongoDBHelper.GetCollection<User>(contextDB, UserCollection, query, Fields.Null);

        }


        internal static User GetUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            IMongoQuery query = Query.EQ("NTLogin", username);

            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            var list = MongoDBHelper.GetCollection<User>(contextDB, UserCollection, query, Fields.Null);
            if (list != null)
                return list.FirstOrDefault();
            else
                return null;

        }

        

        internal static User GetUserByMail(string Email)
        {
            IMongoQuery query = Query.EQ("Email", Email);

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
            if (objUser == null) return false;
            return (objUser.IsHelpDesk);
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
                .Set("IsAdmin", value.IsAdmin)
                .Set("IsHelpDesk", value.IsAdmin);

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
                if (!objUser.IsHelpDesk)
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

        internal static bool UpdatetTicket(string id, Ticket value, string username)
        {
            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            var objUser = GetUser(username);

            //var listComments = (from comm in value.Comments
            //                    //where string.IsNullOrEmpty(comm.Id)
            //                    select new BsonDocument {
            //                    //{ "Id", comm.Id },
            //                    { "Title", comm.Title },
            //                    { "DateCreation", comm.DateCreation },
            //                    { "InsertBy", comm.InsertBy },
            //                    { "Text", comm.Text }
            //                    }).ToArray();
            IMongoUpdate update;
            if (objUser.IsAdmin)
            {
                update = Update
                   .Set("AssignedUser", value.AssignedUser)
                   .Set("Closed", value.Closed)
                   .Set("DateClosed", value.DateClosed)
                   .Set("Description", value.Description)
                   .Set("Title", value.Title)
                   .Set("IsRead", false);
            }
            else
            {
                update = Update
                        .Set("AssignedUser", value.AssignedUser)
                        .Set("Closed", value.Closed)
                        .Set("DateClosed", value.DateClosed)
                        .Set("Description", value.Description)
                        .Set("Title", value.Title);
            }

            

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
                { "Text", value.Text }, 
                { "IsRead", value.IsRead }, 
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

        internal static Notify GetNotifyByUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            var user = TTRepository.GetUser(username);
            if(user == null) return null;

            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            IMongoQuery query;

            if (user.IsAdmin) //controllo se l'utente è ADMIN in questo caso devo estrarre tutti i nuovi ticket inseriti dagli utenti e non ancora gestiti
            {
                query = Query.And(
                    Query.EQ("IsRead", false),
                    Query.NE("SubmittedBy", username));

            }
            else
            {
                if (user.IsHelpDesk) //se help desk, tutti i nuovi assegnati e i commenti nuovi
                {
                    query = Query.Or(
                                Query.And(
                                    Query.EQ("IsRead", false),
                                    Query.EQ("AssignedUser", username),
                                    Query.NE("SubmittedBy", username)),
                                Query.And(
                                    Query.EQ("AssignedUser", username),
                                    Query.Not(Query.Size("Comments", 0)),
                                    Query.ElemMatch("Comments", Query.And(
                                                   Query.EQ("IsRead", false),
                                                   Query.NE("InsertBy", username)
                                                )))
                                    );

                }
                else//se user solo i nuovi commenti
                {
                    query = Query.And(
                        Query.EQ("SubmittedBy", username)
                        , Query.Not(Query.Size("Comments", 0))
                        , Query.ElemMatch("Comments", Query.And(
                                                            Query.EQ("IsRead", false),
                                                            Query.NE("InsertBy", username)
                                                            )
                                            ));
 
                }
            }

            var Listticket = MongoDBHelper.GetCollection<Ticket>(contextDB, TicketCollection, query, Fields.Null);
            if (Listticket == null)
                return null;
            else
            {
                var objNotify = new Notify();
                if (user.IsAdmin)
                {
                    objNotify.NumberTickets = Listticket.Count();
                    objNotify.NumberComments = 0;
                }
                else
                {
                    if (user.IsHelpDesk)
                    {
                        objNotify.NumberTickets = (from l in Listticket
                                                   where l.IsRead == false
                                                   && l.AssignedUser == user.NTLogin
                                                   && l.SubmittedBy != user.NTLogin
                                                   select l).Count();

                        objNotify.NumberComments = (from l in Listticket
                                                    where l.IsRead == false
                                                    && l.AssignedUser == user.NTLogin
                                                    && l.Comments.Where(x => x.IsRead == false && x.InsertBy != user.NTLogin).Count() > 0
                                                    select l).Count();
                    }
                    else
                    {
                        objNotify.NumberComments = (from l in Listticket
                                                    where l.SubmittedBy == user.NTLogin
                                                    && l.Comments.Where(x => x.IsRead == false && x.InsertBy != user.NTLogin).Count() > 0
                                                    select l).Count();
                        objNotify.NumberTickets = 0;

                    }
                }
                return objNotify;

            }

            
        }

        internal static void SetReadTicket(string idTicket, string username)
        {

            if (string.IsNullOrEmpty(username)) return;
            var user = TTRepository.GetUser(username);
            if (user == null) return;

            var contextDB = MongoDBHelper.GetContext(ConnectionString, DB);
            IMongoUpdate update;
            IMongoQuery query = Query.Null;

            if (user.IsAdmin)
            {
                query = Query.And(
                    Query.EQ("_id", new BsonObjectId(new ObjectId(idTicket))),
                    Query.EQ("IsRead", false),
                    Query.NE("SubmittedBy", username));

                update = Update
                    .Set("IsRead", true);

                MongoDBHelper.Modify(contextDB, TicketCollection, query, update, UpdateFlags.Multi);
            }
            else
            {
                if (user.IsHelpDesk)
                {
                    query = Query.And(
                                Query.EQ("_id", new BsonObjectId(new ObjectId(idTicket))),
                                Query.EQ("AssignedUser", username),
                                Query.Not(Query.Size("Comments", 0)),
                                Query.ElemMatch("Comments", Query.And(
                                                Query.EQ("IsRead", false),
                                                Query.NE("InsertBy", username)
                                            ))
                                );

                    update = Update
                        .Set("Comments.$.IsRead", true);

                    MongoDBHelper.Modify(contextDB, TicketCollection, query, update, UpdateFlags.Multi);


                    update = Update
                        .Set("IsRead", true);

                    query = Query.And(
                                   Query.EQ("_id", new BsonObjectId(new ObjectId(idTicket))),
                                   Query.EQ("IsRead", false),
                                   Query.EQ("AssignedUser", username),
                                   Query.NE("SubmittedBy", username));

                    MongoDBHelper.Modify(contextDB, TicketCollection, query, update, UpdateFlags.Multi);

                }
                else
                {
                    query = Query.And(
                        Query.EQ("_id", new BsonObjectId(new ObjectId(idTicket))),
                        Query.ElemMatch("Comments", Query.And(
                                                            Query.EQ("IsRead", false),
                                                            Query.NE("InsertBy", username)
                                                            ))
                        );

                    update = Update
                        .Set("Comments.$.IsRead", true);

                    MongoDBHelper.Modify(contextDB, TicketCollection, query, update, UpdateFlags.Multi);
                }
            }

            
        }
    }
}