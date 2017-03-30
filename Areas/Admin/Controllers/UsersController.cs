using System;
using ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using NHibernate.Linq;


namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.Controllers
{
      [Authorize(Roles = "admin, client, support")]
    [SelectedTab("users")]
    public class UsersController : Controller
    {

        private const int UsersPerPage= 5;
        public ActionResult Index(string searchBy, string search, int page = 1)
        {
            var totalUserCount = Database.Session.Query<User>().Count();
            var currentUserPage = Database.Session.Query<User>();
            if (searchBy == "Username")
            {
                currentUserPage =currentUserPage.Skip((page - 1) * UsersPerPage);
                 currentUserPage =currentUserPage.Take(UsersPerPage);
                 currentUserPage = currentUserPage.Where(t => t.Username.StartsWith(search) || search == null);
                 currentUserPage.ToList();
            }
            else if (searchBy =="Email")
            {
                currentUserPage = currentUserPage.Skip((page - 1) * UsersPerPage);
                currentUserPage = currentUserPage.Take(UsersPerPage);
                currentUserPage = currentUserPage.Where(t => t.Email.StartsWith(search) || search == null);
                currentUserPage.ToList();
            }
            else
            {
                currentUserPage = currentUserPage.Skip((page - 1) * UsersPerPage);
                currentUserPage = currentUserPage.Take(UsersPerPage);
                currentUserPage.ToList();
            }
            return View(new UsersIndex 
            {
                Users = new PagedData<User>(currentUserPage, totalUserCount, page, UsersPerPage)
            });
        }
       
        public ActionResult New()
        {
            return View(new UsersNew
            {
                
                Roles = Database.Session.Query<Role>().Select(role => new RoleCheckbox
                {
                    Id = role.Id,
                    IsChecked = false,
                    Name = role.Name
                }).ToList(),
                Departments = Database.Session.Query<Department>().Select(department => new DepartmentCheckbox()
                {
                    Id = department.Id,
                    IsChecked = false,
                    Name = department.Name
                }).ToList(),
            });
        }

         [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(UsersNew form)
        {
            var user = new User();
            SyncRoles(form.Roles, user.Roles);
            SyncDepartments(form.Departments, user.Departments);

            if (Database.Session.Query<User>().Any(u => u.Username == form.Username))
                ModelState.AddModelError("Username", "Username must be unique");
            if (!ModelState.IsValid)
                return View(form);
             
       
                user.Email = form.Email;
                user.Username = form.Username;
                user.ProfilePicture = form.ProfilePicture;
                user.SetPassword(form.Password);

            Database.Session.Save(user);
            
             TempData["Message"] = "Created User " + user.Username;
            return RedirectToAction("index");
        }

         public ActionResult Edit(int id)                                         
         {
             var user = Database.Session.Load<User>(id);
             if (user == null)
                 return HttpNotFound();

             return View(new UsersEdit
             {
                 Username = user.Username,
                 Email = user.Email,
                 ProfilePicture=user.ProfilePicture,
                 Roles = Database.Session.Query<Role>().Select(role => new RoleCheckbox
                 {
                     Id = role.Id,
                     IsChecked = user.Roles.Contains(role),
                     Name = role.Name
                 }).ToList(),
                 Departments = Database.Session.Query<Department>().Select(department => new DepartmentCheckbox()
                 {
                     Id = department.Id,
                     IsChecked = user.Departments.Contains(department),
                     Name = department.Name
                 }).ToList()
             });
         }

        [HttpPost, ValidateAntiForgeryToken]
         public ActionResult Edit(int id, UsersEdit form)
         {
             var user = Database.Session.Load<User>(id);
             if (user == null)
                 return HttpNotFound();

             SyncRoles(form.Roles, user.Roles);
             SyncDepartments(form.Departments, user.Departments);

             if (Database.Session.Query<User>().Any(u => u.Username == form.Username && u.Id != id))
                 ModelState.AddModelError("Username", "Username must be unique");
             if (!ModelState.IsValid)
                 return View(form);

             user.Username = form.Username;
             user.Email = form.Email;
            user.ProfilePicture = form.ProfilePicture;
             Database.Session.Update(user);

             return RedirectToAction("index");
         }

        public ActionResult ResetPassword(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();

            return View(new UsersResetPassword
            {
                Username = user.Username
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int id, UsersResetPassword form)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();

            form.Username = user.Username;

            if (!ModelState.IsValid)
                return View(form);

            user.SetPassword(form.Password);
            Database.Session.Update(user);

            return RedirectToAction("index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();

            Database.Session.Delete(user);
            return RedirectToAction("index");
        }

        public FileResult ExportTo(string ReportType)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/Users.rdlc");

            SqlConnection con = new SqlConnection(@"Data Source=MWASHUMA\JONNYOLO; Initial Catalog=TICKETINGSYSTEMDB; User ID=sa; Password=6927Yoloo;");
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from users";
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            reportDataSource.Value = dt;

            localReport.DataSources.Add(reportDataSource);

            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtention = (ReportType == "Excel") ? "xlsx" : "pdf";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType, "", out mimeType, out encoding, out fileNameExtention,
                out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment; filename=All Users." + fileNameExtention);

            return File(renderedBytes, fileNameExtention);
        }

        public void SyncRoles(IList<RoleCheckbox> checkboxes, IList<Role> roles)
        {
            var selectedRoles = new List<Role>();

            foreach (var role in Database.Session.Query<Role>())
            {
                var checkbox = checkboxes.Single(c => c.Id == role.Id);
                checkbox.Name = role.Name;

                if (checkbox.IsChecked)
                    selectedRoles.Add(role);
            }

            foreach( var toAdd in selectedRoles.Where(t => !roles.Contains(t)))
                roles.Add(toAdd);

            foreach(var toRemove in roles.Where(t => !selectedRoles.Contains(t)).ToList())
                roles.Remove(toRemove);
        }

        public void SyncDepartments(IList<DepartmentCheckbox> checkboxes, IList<Department> departments)
        {
            var selectedDepartments = new List<Department>();

            foreach (var department in Database.Session.Query<Department>())
            {
                var checkbox = checkboxes.Single(c => c.Id == department.Id);
                checkbox.Name = department.Name;

                if (checkbox.IsChecked)
                    selectedDepartments.Add(department);
            }

            foreach (var toAdd in selectedDepartments.Where(t => !departments.Contains(t)))
                departments.Add(toAdd);

            foreach (var toRemove in departments.Where(t => !selectedDepartments.Contains(t)).ToList())
                departments.Remove(toRemove);
        }
    }
}
