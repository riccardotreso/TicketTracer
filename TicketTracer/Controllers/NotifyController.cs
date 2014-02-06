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
    public class NotifyController : ApiController
    {
        // GET api/notify
        public Notify Get()
        {
            string username = User.Identity.Name;
            return TTRepository.GetNotifyByUser(username);
        }

        // GET api/notify/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/notify
        public void Post([FromBody]string value)
        {
        }

        // PUT api/notify/5
        public void Put(string id)
        {
            string username = User.Identity.Name;
            TTRepository.SetReadTicket(id, username);
        }

        // DELETE api/notify/5
        public void Delete(int id)
        {
        }
    }
}
