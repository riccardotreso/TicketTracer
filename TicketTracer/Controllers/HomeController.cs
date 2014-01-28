using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;

namespace TicketTracer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public const string UserSession = "sessionUser";
        
        
        public ActionResult User()
        {
            return View();
        }

        
        public ActionResult Ticket()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            
            var e = HttpContext.User.Identity;
            return View();
        }


    }
}
