using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Infrastructure.Extensions;
using ITSUPPORTTICKETSYSTEM.Models;
using Microsoft.Reporting.WebForms;
using NHibernate.Linq;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,  support, client")]
    [SelectedTab("ResolvedTickets")]
    public class ResolvedTicketsController : Controller
    {
        private const int TicketsPerPage = 5; 
        //
        // GET: /Admin/ResolvedTickets/


        public ActionResult Index(string searchBy, string search, int page = 1)
        {
            var totalTicketCount = Database.Session.Query<Ticket>().Count(t => t.Status.Any(a => a.Name == "Resolved"));
         
            var currentTicketPage = Database.Session.Query<Ticket>();
            if (User.IsInRole("client"))
            {
                currentTicketPage = currentTicketPage.Where(t => t.User.Username == User.Identity.Name);
                // currentTicketPage = currentTicketPage.Where(t => t.User.Roles.Any(a => a.Name == "support"));
                currentTicketPage = currentTicketPage.Where(t => t.Status.Any(a => a.Name == "Resolved"));
                currentTicketPage = currentTicketPage.Where(t => t.DeletedAt == null);
                currentTicketPage = currentTicketPage.OrderByDescending(c => c.CreatedAt);
                currentTicketPage = currentTicketPage.Skip((page - 1) * TicketsPerPage);
                currentTicketPage = currentTicketPage.Take(TicketsPerPage);
                currentTicketPage.ToList();
            }
            else if (searchBy == "Title")
            {
                currentTicketPage = currentTicketPage.Where(t => t.Status.Any(a => a.Name == "Resolved"));
                currentTicketPage = currentTicketPage.Where(t => t.DeletedAt == null);
                currentTicketPage = currentTicketPage.Where(emp => emp.Title.StartsWith(search) || search == null);
                currentTicketPage = currentTicketPage.OrderByDescending(c => c.CreatedAt);
                currentTicketPage = currentTicketPage.Skip((page - 1) * TicketsPerPage);
                currentTicketPage = currentTicketPage.Take(TicketsPerPage);
                currentTicketPage.ToList();
            }
            else if (searchBy == "Username")
            {
                currentTicketPage = currentTicketPage.Where(t => t.Status.Any(a => a.Name == "Resolved"));
                currentTicketPage = currentTicketPage.Where(t => t.DeletedAt == null);
                currentTicketPage = currentTicketPage.Where(emp => emp.User.Username.StartsWith(search) || search == null);
                currentTicketPage = currentTicketPage.OrderByDescending(c => c.CreatedAt);
                currentTicketPage = currentTicketPage.Skip((page - 1) * TicketsPerPage);
                currentTicketPage = currentTicketPage.Take(TicketsPerPage);
                currentTicketPage.ToList();
            }
            else
            {
                currentTicketPage = currentTicketPage.Where(t => t.Status.Any(a => a.Name == "Resolved"));
                currentTicketPage = currentTicketPage.Where(t => t.DeletedAt == null);
                currentTicketPage = currentTicketPage.OrderByDescending(c => c.CreatedAt);
                currentTicketPage = currentTicketPage.Skip((page - 1)*TicketsPerPage);
                currentTicketPage = currentTicketPage.Take(TicketsPerPage);
                currentTicketPage.ToList();
            }

            return View(new ResolvedTicketsIndex()
            {
                Tickets = new PagedData<Ticket>(currentTicketPage, totalTicketCount, page, TicketsPerPage)
            });
        }

        
            public ActionResult New()
        {

            return View("Form", new TicketsForm
            {
                IsNew = true,
                
                Status = Database.Session.Query<TicketStatus>().Select(ticketstatus => new StatusCheckbox
                {
                    Id = ticketstatus.Id,
                    IsChecked = false,
                    Name = ticketstatus.Name
                }).ToList(),

                Tags = Database.Session.Query<Tag>().Select(tag => new TagCheckbox
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    IsChecked = false
                }).ToList(),

                Comments = Database.Session.Query<Comments>().Select(comment => new CommentCheckbox()
                {
                    Id = comment.Id,
                    IsChecked = false,
                    Contents = comment.Contents
                }).ToList()
            });
        }

        public ActionResult Edit(int id)
        {
            var ticket = Database.Session.Load<Ticket>(id);
            if (ticket == null)
                return HttpNotFound();
            return View("Form", new TicketsForm
            {
                IsNew = false,
                TicketId = id,
                Content = ticket.Content,
                Slug = ticket.Slug,
                Title = ticket.Title,

                Status = Database.Session.Query<TicketStatus>().Select(ticketstatus => new StatusCheckbox
                {
                    Id = ticketstatus.Id,
                    IsChecked = ticket.Status.Contains(ticketstatus),
                    Name = ticketstatus.Name
                }).ToList(),

                Tags = Database.Session.Query<Tag>().Select(tag => new TagCheckbox
                {
                    Id = tag.Id,
                    IsChecked = ticket.Tags.Contains(tag),
                    Name = tag.Name
                }).ToList(),
                Comments = Database.Session.Query<Comments>().Select(comment => new CommentCheckbox()
                {
                    Id = comment.Id,
                    IsChecked = ticket.Comments.Contains(comment),
                    Contents = comment.Contents
                }).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken,ValidateInput(false)]
        public ActionResult Form(TicketsForm form)
        {
            var ticket = new Ticket();
           
            form.IsNew = form.TicketId == null;

            if (!ModelState.IsValid)
                return View(form);

            var selectedTags = ReconsileTags(form.Tags).ToList();
            var selectedComments = ReconsileComments(form.Comments).ToList();

            
            if (form.IsNew)
           {
               ticket.CreatedAt = DateTime.UtcNow;
               ticket.User = Auth.User;

               foreach (var tag in selectedTags)
               {
                   ticket.Tags.Add(tag);
               }
               foreach (var comment in selectedComments)
               {
                   ticket.Comments.Add(comment);
                   //ticket.User = Auth.User;
               }
                
            }
     
            else
            {
                ticket = Database.Session.Load<Ticket>(form.TicketId);
                if (ticket == null)
                    return HttpNotFound();
                ticket.UpdatedAt = DateTime.UtcNow;

                foreach (var toAdd in selectedTags.Where(t => !ticket.Tags.Contains(t)))
                    ticket.Tags.Add(toAdd);

                foreach (var toRemove in ticket.Tags.Where(t => !selectedTags.Contains(t)).ToList())
                    ticket.Tags.Remove(toRemove);

                foreach (var toAdd in selectedComments.Where(t => !ticket.Comments.Contains(t)))
                    ticket.Comments.Add(toAdd);

                foreach (var toRemove in ticket.Comments.Where(t => !selectedComments.Contains(t)).ToList())
                    ticket.Comments.Remove(toRemove);
               
            }
            SyncStatus(form.Status, ticket.Status);
            ticket.Title = form.Title;
            ticket.Slug = form.Slug;
            ticket.Content = form.Content;
            
            
            Database.Session.SaveOrUpdate(ticket);

            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Trash(int id)
        {
            var ticket = Database.Session.Load<Ticket>(id);
            if (ticket == null)
                return HttpNotFound();

            ticket.DeletedAt = DateTime.UtcNow;
            Database.Session.Update(ticket);
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var ticket = Database.Session.Load<Ticket>(id);
            if (ticket == null)
                return HttpNotFound();

            Database.Session.Delete(ticket);
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Restore(int id)
        {
            var ticket = Database.Session.Load<Ticket>(id);
            if (ticket == null)
                return HttpNotFound();

            ticket.DeletedAt = null;
            Database.Session.Update(ticket);
            return RedirectToAction("Index");
        }

        public FileResult ExportTo(string ReportType)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/TicketReport.rdlc");

            SqlConnection con = new SqlConnection(@"Data Source=MWASHUMA\JONNYOLO; Initial Catalog=TICKETINGSYSTEMDB; User ID=sa; Password=6927Yoloo;");
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from tickets;select * from tags";
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
            Response.AddHeader("content-disposition", "attachment; filename=Urls." + fileNameExtention);

            return File(renderedBytes, fileNameExtention);
        }
         
        public void SyncStatus(IList<StatusCheckbox> checkboxes, IList<TicketStatus> status)
        {
            var selectedStatus = new List<TicketStatus>();
            
            foreach (var ticketstatus in Database.Session.Query<TicketStatus>())
            {
                var checkbox = checkboxes.Single(c => c.Id == ticketstatus.Id);
                checkbox.Name = ticketstatus.Name;

                if (checkbox.IsChecked)
                    selectedStatus.Add(ticketstatus);
            }

            foreach (var toAdd in selectedStatus.Where(ticket => !status.Contains(ticket)))
                status.Add(toAdd);

            foreach (var toRemove in status.Where(t => !selectedStatus.Contains(t)).ToList())
                status.Remove(toRemove);
        }

        private IEnumerable<Tag> ReconsileTags(IEnumerable<TagCheckbox> tags)
        {
            foreach (var tag in tags.Where(t => t.IsChecked))
            {
                if (tag.Id != null)
                {
                    yield return Database.Session.Load<Tag>(tag.Id);
                    continue;
                }
                var existingTag = Database.Session.Query<Tag>().FirstOrDefault(t => t.Name == tag.Name);
                if (existingTag != null)
                {
                    yield return existingTag;
                    continue;
                }

                var newTag = new Tag
                {
                    Name = tag.Name,
                    Slug = tag.Name.Slugify()
                };
                Database.Session.Save(newTag);
                yield return newTag;

            }
        }

        private IEnumerable<Comments> ReconsileComments(IEnumerable<CommentCheckbox> comments)
        {
            foreach (var comment in comments.Where(t => t.IsChecked))
            {
                if (comment.Id != null)
                {
                    yield return Database.Session.Load<Comments>(comment.Id);
                    continue;
                }
                var existingComment = Database.Session.Query<Comments>().FirstOrDefault(t => t.Contents == comment.Contents);
                if (existingComment != null)
                {
                    yield return existingComment;
                    continue;
                }

                var newComment = new Comments
                {
                    Contents = comment.Contents,
                    User = Auth.User,
                    CreatedAt = DateTime.UtcNow,
                    Slug = comment.Contents.Slugify()
                };
                Database.Session.Save(newComment);
                yield return newComment;

            }
        }


    }

    }

