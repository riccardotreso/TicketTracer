using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketTracer.DTO;
using TicketTracer.Models;

namespace TicketTracer.Controllers
{
    public class CommentController : ApiController
    {
        // GET api/comment
        public IEnumerable<Comment> Get(string idTicket)
        {
            return TTRepository.GetAllTicketComments(idTicket);
        }

        // GET api/comment/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/comment
        public Comment Post(string idTicket, Comment value)
        {
            
            TTRepository.InsertTicketComment(idTicket, value, User.Identity.Name);
            return value;
        }

        // PUT api/comment/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/comment/5
        public void Delete(int id)
        {
        }
    }
}
