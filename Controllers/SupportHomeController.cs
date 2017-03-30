using ITSUPPORTTICKETSYSTEM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels;
using ITSUPPORTTICKETSYSTEM.ViewModels;
using NHibernate.Linq;

namespace ITSUPPORTTICKETSYSTEM.Controllers
{
    public class SupportHomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("home");
        }


        [HttpPost]
        public ActionResult Login(AuthLogin form, string returnUrl)
        {
            //System.Threading.Thread.Sleep(3000);

            //ModelState.Clear();

            var user = Database.Session.Query<User>().FirstOrDefault(u => u.Username == form.Username);
            if (user == null)
                ITSUPPORTTICKETSYSTEM.Models.User.FakeHash();

            if (user == null || !user.CheckPassword(form.Password))
                ModelState.AddModelError("Username", "Username or Password is incorrect");

            if (!ModelState.IsValid)
                return View(form);

            FormsAuthentication.SetAuthCookie(user.Username, true);
            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return RedirectToRoute("Dashboard");
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
       

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int id,AuthLogin form)
        {
            var user = new User(); 
                 user = Database.Session.Query<User>().FirstOrDefault(u => u.Username == form.Username);
            if (form.Username == user.Username)
            {
                user.SetPassword(form.Password);
                Database.Session.Update(user);
            }

            return RedirectToRoute("home");
        }
    }
}