using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketTracer.DTO;
using TicketTracer.Models;
using TicketTracer.Util;

namespace TicketTracer.Controllers
{
    public class UserController : ApiController
    {
        // GET api/user
        public IEnumerable<TicketTracer.DTO.User> Get(bool isHelpDesk)
        {

            return TTRepository.GetAllUser(isHelpDesk);
        }

        // GET api/user/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/user
        public void Post(TicketTracer.DTO.User value)
        {
            value.Password = CryptographyHelper.Encrypt(value.Password);
            TTRepository.InsertUser(value);
        }

        // PUT api/user/5
        public void Put(string id, TicketTracer.DTO.User value)
        {
            TTRepository.UpdatetUser(id, value);
        }

        // DELETE api/user/5
        public void Delete(string id)
        {
            TTRepository.DeleteUser(id);
        }
    }
}
