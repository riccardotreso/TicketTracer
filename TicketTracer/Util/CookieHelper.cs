using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketTracer.Util
{
    public class CookieHelper
    {
        private static HttpCookie myCookie;
        private static string CookieName;
        private static int cookieExpireDate;

        public CookieHelper CreateCookie(string Name, int cookieExpireDate = 30)
        {
            myCookie = new HttpCookie(Name);
            myCookie.Expires = DateTime.Now.AddDays(cookieExpireDate);
            HttpContext.Current.Response.Cookies.Add(myCookie);
            return this;
 
        }

        public void DeleteCookie(string Name)
        {
            HttpContext.Current.Response.Cookies.Remove(Name);
        }

        public CookieHelper SetCookie(string key, string value)
        {
            myCookie[key] = value;
            return this;
        }

        public bool Exist(string Name)
        {
            myCookie = HttpContext.Current.Request.Cookies[Name];
            if (myCookie == null)
                return false;

            return true;

        }

    }
}