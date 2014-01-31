using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using TicketTracer.DTO;
using TicketTracer.Models;

namespace TicketTracer.Util
{
    public class SecurityHelper
    {
        
        private static string cookieName = "TicketTracesCookie";


        public static void InitDefaultAccount()
        {
            string userName = "admin";
            string password = "admin";
            if (TTRepository.GetUser(userName) == null)
            {
                User u = new User()
                {
                    Name = "Administrator",
                    Surname = "Administrator",
                    Email = "foo@tickettracer.it",
                    NTLogin = userName,
                    Password = CryptographyHelper.Encrypt(password),
                    IsAdmin = true,
                    Enabled = true,
                    Division = "Administrator"
                };
                TTRepository.InsertUser(u);
            }
        }

        public static void Logout()
        {
            CookieHelper cookie = new CookieHelper();
            cookie.DeleteCookie(cookieName);
            FormsAuthentication.SignOut();
        }


        public static bool Login(string UserName, string Password, bool createPersistentCookie = false)
        {
            string encryptPassword = CryptographyHelper.Encrypt(Password);
            CookieHelper cookie = new CookieHelper();

            var objUser = TTRepository.GetUser(UserName);
            if (objUser != null && objUser.Password == encryptPassword)//Utente presente controllo il cookie
            {
                if (!cookie.Exist(cookieName))
                {
                    cookie.CreateCookie(cookieName)
                        .SetCookie("UserName", UserName)
                        .SetCookie("Password", encryptPassword);
                }

                Authenticate(UserName, createPersistentCookie);
                return true;
            }
            return false;
        }

        private static void Authenticate(string UserName,bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(UserName, createPersistentCookie);

            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(UserName), null);

        }


        public static bool CreateUserAndAccount(RegisterModel model)
        {
            User u = new User()
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                NTLogin = model.UserName,
                Password = CryptographyHelper.Encrypt(model.Password)
            };

            var objUser = TTRepository.GetUser(model.UserName);
            if (objUser != null)
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);

            objUser = TTRepository.GetUserByMail(model.Email);
            if (objUser != null)
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateEmail);

            var id = TTRepository.InsertUser(u);

            return true;



        }
    }
}