using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketTracer.Models;

namespace TicketTracer.Controllers
{
    public class TicketController : ApiController
    {
        // GET api/ticket
        public IEnumerable<TicketTracer.DTO.Ticket> Get()
        {
            string username = User.Identity.Name;
            return TTRepository.GetAllTicket(username);
        }

        // GET api/ticket/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/ticket
        public void Post(TicketTracer.DTO.Ticket value)
        {
            value.SubmittedBy = User.Identity.Name;
            TTRepository.InsertTicket(value);
        }

        // PUT api/ticket/5
        public void Put(string id, TicketTracer.DTO.Ticket value)
        {
            TTRepository.UpdatetTicket(id, value);
        }

        // DELETE api/ticket/5
        public void Delete(string id)
        {
            TTRepository.DeleteTicket(id);
        }
    }
}
