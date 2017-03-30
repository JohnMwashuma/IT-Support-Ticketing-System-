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
    [SelectedTab("departments")]
    public class DepartmentController : Controller
    {
        private const int DepartmentsPerPage = 3;
        //
        // GET: /Admin/Department/

        public ActionResult Index(int page = 1)
        {
            var totalDepartmentsCount = Database.Session.Query<Department>().Count();
            //var currentTicketPage = Database.Session.Query<Ticket>()
            //     //.Where(t => t.Status.Any(a => a.Name == "New"))
            //     //.Where(t => t.User.Username == User.Identity.Name)
            //     .OrderByDescending(c => c.CreatedAt)
            //     .Skip((page - 1)*TicketsPerPage)
            //     .Take(TicketsPerPage)
            //     .ToList();
            var currentDepartmentPage = Database.Session.Query<Department>();
            if (User.IsInRole("client"))
            {
                totalDepartmentsCount = Database.Session.Query<Ticket>().Count(t => t.User.Username == User.Identity.Name);
                // currentTicketPage = currentTicketPage.Where(t => t.User.Roles.Any(a => a.Name == "support"));
                //currentTicketPage = currentTicketPage.Where(t => t.Status.Any(a => a.Name == "New"));
                currentDepartmentPage = currentDepartmentPage.OrderByDescending(c => c.CreatedAt);
                currentDepartmentPage = currentDepartmentPage.Skip((page - 1) * DepartmentsPerPage);
                currentDepartmentPage = currentDepartmentPage.Take(DepartmentsPerPage);
                currentDepartmentPage.ToList();
            }
            else
            {
                currentDepartmentPage = currentDepartmentPage.OrderByDescending(c => c.CreatedAt);
                currentDepartmentPage = currentDepartmentPage.Skip((page - 1) * DepartmentsPerPage);
                currentDepartmentPage = currentDepartmentPage.Take(DepartmentsPerPage);
                currentDepartmentPage.ToList();
            }

            return View(new Departmentsform()
            {
                Departments = new PagedData<Department>(currentDepartmentPage, totalDepartmentsCount, page, DepartmentsPerPage)
            });
        }

        public ActionResult New()
        {

            return View("Index", new Departmentsform
            {
                IsNew = true
            });
        }

        public ActionResult Edit(int id)
        {
            var department = Database.Session.Load<Department>(id);
            if (department == null)
                return HttpNotFound();
            return View("Index", new Departmentsform
            {

                IsNew = false,
                DepartmentId = id,
                Name = department.Name
            });
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Index(Departmentsform index)
        {
            var department = new Department();

            index.IsNew = index.DepartmentId == null;

            if (!ModelState.IsValid)
                return View(index);



            department.Name = index.Name;
            department.CreatedAt = DateTime.UtcNow;


            Database.Session.SaveOrUpdate(department);

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
            var department = Database.Session.Load<Department>(id);
            if (department == null)
                return HttpNotFound();

            Database.Session.Delete(department);
            return RedirectToAction("Index");
        }

    }
}
