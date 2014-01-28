using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketTracer.DTO
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NTLogin { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; }
        public string Division { get; set; }
        public bool IsAdmin { get; set; }

        public string FullName()
        {
            return Name + " " + Surname;
        }
    }
}