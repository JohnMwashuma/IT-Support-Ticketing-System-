using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels;
using ITSUPPORTTICKETSYSTEM.Models;
using NHibernate.Linq;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.Controllers
{
    public class LayoutController : Controller
    {
        //
        // GET: /Admin/Layout/
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            return View( new LayoutSidebar
            {
                IsLoggedIn = Auth.User !=null,
                UserName = Auth.User != null ? Auth.User.Username : "",
                IsAdmin = User.IsInRole("admin"),
                Tags = Database.Session.Query<Tag>().Select(tag => new
                {
                    tag.Id,
                    tag.Name,
                    tag.Slug,
                    TicketCount=tag.Tickets.Count
                }).Where(t => t.TicketCount >0).OrderByDescending(p => p.TicketCount).Select(tag => new SidebarTag(tag.Id,tag.Name,tag.Slug,tag.TicketCount)).ToList()
            });
        }

    }
}
