using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketTracer.DTO
{
    public class Ticket : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateClosed { get; set; }
        public bool Closed { get; set; }
        public string AssignedUser { get; set; }
        public string SubmittedBy { get; set; }
        public string SubmittedEmail { get; set; }
        public bool IsRead { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }

    public class Comment
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateCreation { get; set; }
        public string InsertBy { get; set; }
        public bool IsRead { get; set; }


    }

    public class Notify
    {
        public int NumberTickets { get; set; }
        public int NumberComments { get; set; } 
    }
}