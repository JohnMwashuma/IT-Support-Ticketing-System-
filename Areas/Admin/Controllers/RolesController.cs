using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;
using NHibernate.Linq;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,  support, client")]
    [SelectedTab("roles")]
    public class RolesController : Controller
    {
        private const int RolesPerPage = 3;
        //
        // GET: /Admin/Roles/

        public ActionResult Index(int page = 1)
        {
            var totalRoleCount = Database.Session.Query<Role>().Count();
            //var currentTicketPage = Database.Session.Query<Ticket>()
            //     //.Where(t => t.Status.Any(a => a.Name == "New"))
            //     //.Where(t => t.User.Username == User.Identity.Name)
            //     .OrderByDescending(c => c.CreatedAt)
            //     .Skip((page - 1)*TicketsPerPage)
            //     .Take(TicketsPerPage)
            //     .ToList();
            var currentRolePage = Database.Session.Query<Role>();
            if (User.IsInRole("client"))
            {
                totalRoleCount = Database.Session.Query<Ticket>().Count(t => t.User.Username == User.Identity.Name);
                // currentTicketPage = currentTicketPage.Where(t => t.User.Roles.Any(a => a.Name == "support"));
                //currentTicketPage = currentTicketPage.Where(t => t.Status.Any(a => a.Name == "New"));
                currentRolePage = currentRolePage.OrderByDescending(c => c.CreatedAt);
                currentRolePage = currentRolePage.Skip((page - 1) * RolesPerPage);
                currentRolePage = currentRolePage.Take(RolesPerPage);
                currentRolePage.ToList();
            }
            else
            {
                currentRolePage = currentRolePage.OrderByDescending(c => c.CreatedAt);
                currentRolePage = currentRolePage.Skip((page - 1) * RolesPerPage);
                currentRolePage = currentRolePage.Take(RolesPerPage);
                currentRolePage.ToList();
            }

            return View(new Rolesform()
            {
                Roles = new PagedData<Role>(currentRolePage, totalRoleCount, page, RolesPerPage)
            });
        }

        public ActionResult New()
        {

            return View("Index", new Rolesform
            {
                IsNew = true
            });
        }

        public ActionResult Edit(int id)
        {
            var role = Database.Session.Load<Role>(id);
            if (role == null)
                return HttpNotFound();
            return View("Index", new Rolesform
            {

                IsNew = false,
                RoleId = id,
                Name = role.Name
            });
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Index(Rolesform index)
        {
            var role = new Role();

            index.IsNew = index.RoleId == null;

            if (!ModelState.IsValid)
                return View(index);



            role.Name = index.Name;
            role.CreatedAt = DateTime.UtcNow;
          

            Database.Session.SaveOrUpdate(role);

            return RedirectToAction("Index");
        }


        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult Trash(int id)
        //{
        //    var ticket = Database.Session.Load<Ticket>(id);
        //    if (ticket == null)
        //        return HttpNotFound();

        //    ticket.DeletedAt = DateTime.UtcNow;
        //    Database.Session.Update(ticket);
        //    return RedirectToAction("Index");
        //}

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var role = Database.Session.Load<Role>(id);
            if (role == null)
                return HttpNotFound();

            Database.Session.Delete(role);
            return RedirectToAction("Index");
        }

    }
}
