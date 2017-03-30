using ITSUPPORTTICKETSYSTEM.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels;
using ITSUPPORTTICKETSYSTEM.Infrastructure.Extensions;
using ITSUPPORTTICKETSYSTEM.Models;
using Microsoft.Reporting.WebForms;
using NHibernate.Linq;
using System.Data.SqlClient;
using System.Data;
using NHibernate.Type;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.Controllers
{
    [Authorize(Roles="admin,  support, client")]
    [SelectedTab("tickets")]
    public class TicketsController : Controller
    {
        private const int TicketsPerPage = 5;

        public ActionResult Index(string searchBy, string search, int page = 1)
        {
            int totalTicketCount;
           //var currentTicketPage = Database.Session.Query<Ticket>()
           //     //.Where(t => t.Status.Any(a => a.Name == "New"))
           //     //.Where(t => t.User.Username == User.Identity.Name)
           //     .OrderByDescending(c => c.CreatedAt)
           //     .Skip((page - 1)*TicketsPerPage)
           //     .Take(TicketsPerPage)
           //     .ToList();
            var currentTicketPage = Database.Session.Query<Ticket>();
           // if (User.IsInRole("client") ) 
           // {
           // totalTicketCount = Database.Session.Query<Ticket>().Count(t => t.User.Username == User.Identity.Name);
           // currentTicketPage = currentTicketPage.Where(t => t.User.Username == User.Identity.Name);
           //// currentTicketPage = currentTicketPage.Where(t => t.User.Roles.Any(a => a.Name == "support"));
           // //currentTicketPage = currentTicketPage.Where(t => t.Status.Any(a => a.Name == "New"));
           // currentTicketPage = currentTicketPage.OrderByDescending(c => c.CreatedAt);
           // currentTicketPage = currentTicketPage.Skip((page - 1)*TicketsPerPage);
           // currentTicketPage = currentTicketPage.Take(TicketsPerPage);
           // currentTicketPage.ToList();
           // }

            if (User.IsInRole("client") )
            {
                totalTicketCount = Database.Session.Query<Ticket>().Where(t => t.DeletedAt == null).Count(t => t.User.Username == User.Identity.Name);
                currentTicketPage = currentTicketPage.Where(t => t.DeletedAt == null);
                currentTicketPage = currentTicketPage.Where(t => t.User.Username == User.Identity.Name);
                currentTicketPage = currentTicketPage.Where(emp => emp.Title.StartsWith(search) || search == null);
                currentTicketPage = currentTicketPage.OrderByDescending(c => c.CreatedAt);
                currentTicketPage = currentTicketPage.Skip((page - 1) * TicketsPerPage);
                currentTicketPage = currentTicketPage.Take(TicketsPerPage);
                currentTicketPage.ToList();
            }
            else if (searchBy == "Title")
            {
                totalTicketCount = Database.Session.Query<Ticket>().Count(t => t.DeletedAt == null);
                currentTicketPage = currentTicketPage.Where(t => t.DeletedAt == null);
                currentTicketPage = currentTicketPage.Where(emp => emp.Title.StartsWith(search) || search == null);
                currentTicketPage = currentTicketPage.OrderByDescending(c => c.CreatedAt);
                currentTicketPage = currentTicketPage.Skip((page - 1) * TicketsPerPage);
                currentTicketPage = currentTicketPage.Take(TicketsPerPage);
                currentTicketPage.ToList();
            }

            else if (searchBy == "Username")
            {
                totalTicketCount = Database.Session.Query<Ticket>().Count(t => t.DeletedAt == null);
                currentTicketPage = currentTicketPage.Where(t => t.DeletedAt == null);
                currentTicketPage = currentTicketPage.Where(emp => emp.User.Username.StartsWith(search) || search == null);
                currentTicketPage = currentTicketPage.OrderByDescending(c => c.CreatedAt);
                currentTicketPage = currentTicketPage.Skip((page - 1) * TicketsPerPage);
                currentTicketPage = currentTicketPage.Take(TicketsPerPage);
                currentTicketPage.ToList();
            }
            else
            {
                totalTicketCount = Database.Session.Query<Ticket>().Count(t => t.DeletedAt == null);
                currentTicketPage = currentTicketPage.Where(t => t.DeletedAt == null);
                currentTicketPage = currentTicketPage.OrderByDescending(c => c.CreatedAt);
                currentTicketPage = currentTicketPage.Skip((page - 1) * TicketsPerPage);
                currentTicketPage = currentTicketPage.Take(TicketsPerPage);
                currentTicketPage.ToList(); 
            }

            return View( new TicketsIndex()
            {
                Tickets = new PagedData<Ticket>(currentTicketPage, totalTicketCount, page, TicketsPerPage)
            });
        }

        //public ActionResult _Search(string searchBy, string search)
        //{
        //    if (searchBy == "Content")
        //    {
        //        var model = Database.Session.Query<Ticket>().Where(emp => emp.Content.StartsWith(search) || search == null).ToList();
        //        return View(new TicketsIndex()
        //        {
        //           model = new   PagedData<Ticket>(model)
        //        }); 
        //    }
        //    else
        //    {
        //        var model = Database.Session.Query<Ticket>().Where(emp => emp.Title.StartsWith(search) || search == null).ToList(); 
        //          return View( model); 
        //    }
        //}

        public ActionResult Tag(string idAndSlug, int page = 1)
        {
            var parts = SeperateIdAndSlug(idAndSlug);
            if (parts == null)
                return HttpNotFound();

            var tag = Database.Session.Load<Tag>(parts.Item1);
            if (tag == null)
                return HttpNotFound();

            if (!tag.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
                return RedirectToRoutePermanent("Tag", new { id = parts.Item1, slug = tag.Slug });

            var totalTicketCount = tag.Tickets.Count();
            var ticketids = tag.Tickets.Skip((page - 1) * TicketsPerPage)
               .OrderByDescending(g => g.CreatedAt)
               .Take(TicketsPerPage)
               .Where(t => t.DeletedAt == null)
               .Select(t => t.Id)
               .ToArray();
            var tickets = Database.Session.Query<Ticket>()
                .OrderByDescending(b => b.CreatedAt)
                .Where(t => ticketids.Contains(t.Id))
                .FetchMany(f => f.Tags)
                .Fetch(f => f.User)
                .ToList();

            return View(new TicketsTag()
            {
                Tag = tag,
                Tickets = new PagedData<Ticket>(tickets, totalTicketCount, page, TicketsPerPage)
            });
        }

        public ActionResult Show(string idAndSlug)
        {
            var parts = SeperateIdAndSlug(idAndSlug);
            if (parts == null)
                return HttpNotFound();

            var ticket = Database.Session.Load<Ticket>(parts.Item1);
            if (ticket == null || ticket.IsDeleted)
                return HttpNotFound();

            if (!ticket.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
                return RedirectToRoutePermanent("Ticket", new { id = parts.Item1, slug = ticket.Slug });

            return View(new TicketsShow
            {
                Ticket = ticket
            });
        }

        private Tuple<int, string> SeperateIdAndSlug(string idAndSlug)
        {
            var matches = Regex.Match(idAndSlug ?? "", @"^(\d+)\-(.*)?$");
            if (!matches.Success)
                return null;

            var id = int.Parse(matches.Result("$1"));
            var slug = matches.Result("$2");

            return Tuple.Create(id, slug);
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

                Priorities = Database.Session.Query<Priority>().Select(ticketpriority => new PriorityCheckbox()
                {
                    Id = ticketpriority.Id,
                   Name = ticketpriority.Name,
                    IsChecked = false
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

        //public ActionResult View(int id)
        //{
        //    var ticket = Database.Session.Load<Ticket>(id);
        //    if (ticket == null)
        //        return HttpNotFound();
        //    return PartialView("formmodal", new TicketsIndex
        //    {
                
        //        IsNew = false,
        //        TicketId = id,
        //        Content = ticket.Content,
        //        Title = ticket.Title,
               

        //        Status = Database.Session.Query<TicketStatus>().Select(ticketstatus => new StatusCheckbox
        //        {
        //            Id = ticketstatus.Id,
        //            IsChecked = ticket.Status.Contains(ticketstatus),
        //            Name = ticketstatus.Name
        //        }).ToList()
        //    });
        //}
        

        public ActionResult Edit(int id)
        {
            var ticket = Database.Session.Load<Ticket>(id);
            if (ticket == null)
                return HttpNotFound();
            if (User.IsInRole("client"))
            {
                return View("Form", new TicketsForm
                {

                    IsNew = false,
                    TicketId = id,
                    Content = ticket.Content,
                    Title = ticket.Title,
                    Slug = ticket.Slug,

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
                    }).ToList(),
                    Priorities = Database.Session.Query<Priority>().Select(tickety => new PriorityCheckbox
                    {
                        Id = tickety.Id,
                        Name = tickety.Name,
                        IsChecked = ticket.Priorities.Contains(tickety)
                    }).ToList()
                });
            }
            else
            {
                return View("Form", new TicketsForm
                {

                    IsNew = false,
                    TicketId = id,
                    Content = ticket.Content,
                    Title = ticket.Title,
                    Slug = ticket.Slug,

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
                    Comments = Database.Session.Query<Comments>().Where(t => t.User.Username == User.Identity.Name).Select(comment => new CommentCheckbox()
                    {
                        Id = comment.Id,
                        IsChecked = ticket.Comments.Contains(comment),
                        Contents = comment.Contents
                    }).ToList(),
                    Priorities = Database.Session.Query<Priority>().Select(tickety => new PriorityCheckbox
                    {
                        Id = tickety.Id,
                        Name = tickety.Name,
                        IsChecked = ticket.Priorities.Contains(tickety)
                    }).ToList()
                });
            }
            
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Form(TicketsForm form)
        {
            var ticket = new Ticket();
          //var user = Database.Session.Load<User>(id);
           
            form.IsNew = form.TicketId == null;

            if (!ModelState.IsValid)
                return View(form);

            var selectedTags = ReconsileTags(form.Tags).ToList();
            var selectedComments = ReconsileComments(form.Comments).ToList();
            var selectedPriority = ReconsilePriority(form.Priorities).ToList();

            
            if (form.IsNew)
           {
               ticket.CreatedAt = DateTime.Now;
               ticket.User = Auth.User;
               //ticket.Department = Auth.User;

               foreach (var tag in selectedTags)
               {
                 ticket.Tags.Add(tag);  
               }
               foreach (var comment in selectedComments)
               {
                   ticket.Comments.Add(comment);
                   //ticket.User = Auth.User;
               }
               foreach (var priority in selectedPriority)
               {
                   ticket.Priorities.Add(priority);
                   //ticket.User = Auth.User;
               }

           }
     
            else
            {
                ticket = Database.Session.Load<Ticket>(form.TicketId);

             if (ticket == null)
                    return HttpNotFound();
                ticket.UpdatedAt = DateTime.Now;

                foreach (var toAdd in selectedTags.Where(t => !ticket.Tags.Contains(t))) 
                    ticket.Tags.Add(toAdd);

                foreach (var toRemove in ticket.Tags.Where(t => !selectedTags.Contains(t)).ToList())
                    ticket.Tags.Remove(toRemove);

                foreach (var toAdd in selectedComments.Where(t => !ticket.Comments.Contains(t)))
                    ticket.Comments.Add(toAdd);

                foreach (var toRemove in ticket.Comments.Where(t => !selectedComments.Contains(t)).ToList())
                    ticket.Comments.Remove(toRemove);

                foreach (var toAdd in selectedPriority.Where(t => !ticket.Priorities.Contains(t)))
                    ticket.Priorities.Add(toAdd);

                foreach (var toRemove in ticket.Priorities.Where(t => !selectedPriority.Contains(t)).ToList())
                    ticket.Priorities.Remove(toRemove);

            }
            SyncStatus(form.Status, ticket.Status);
            //SyncPriority(form.Priorities, ticket.Priorities);
            ticket.Title = form.Title;
            ticket.Content=form.Content;
            ticket.Slug = form.Slug;
        
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
            localReport.ReportPath = Server.MapPath("~/Reports/SupportReports/SupportReports/All Support Tickets.rdl");

            SqlConnection con = new SqlConnection(@"Data Source=MWASHUMA\JONNYOLO; Initial Catalog=TICKETINGSYSTEMDB; User ID=sa; Password=6927Yoloo;");
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = string.Format("SELECT status.name AS [status name], tickets.title, tickets.content, tickets.created_at, users.username, ticketpriorities.name AS [ticketpriorities name], ticketpriority_pivot.ticket_id, ticket_status.[ticket_id ], department_users.user_id, department.name AS [department name] FROM ticketpriority_pivot INNER JOIN ticketpriorities ON ticketpriority_pivot.priority_id = ticketpriorities.id INNER JOIN tickets ON ticketpriority_pivot.ticket_id = tickets.id INNER JOIN users ON tickets.user_id = users.id INNER JOIN department_users ON users.id = department_users.user_id INNER JOIN department ON department_users.department_id = department.id INNER JOIN ticket_status ON tickets.id = ticket_status.[ticket_id ] INNER JOIN status ON ticket_status.status_id = status.id");
            //cmd.CommandText = string.Format("SELECT        tickets.title, tickets.[content], tickets.created_at, status.name FROM tickets INNER JOIN ticket_status ON tickets.id = ticket_status.[ticket_id ] INNER JOIN status ON ticket_status.status_id = status.id");
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
            Warning [] warnings;
            string [] streams;
            byte [] renderedBytes;

            renderedBytes = localReport.Render(reportType, "", out mimeType, out encoding, out fileNameExtention,
                out streams, out warnings);
            Response.AddHeader("content-disposition","attachment; filename=All Tickets." + fileNameExtention);

            return File(renderedBytes, fileNameExtention);
        }
         
        //public void SyncPriority(IList<PriorityCheckbox> checkboxes, IList<Priority> priority)
        //{
        //    var selectedPriority = new List<Priority>();

        //    foreach (var ticketpriority in Database.Session.Query<Priority>())
        //    {
        //        var checkbox = checkboxes.Single(c => c.Id == ticketpriority.Id);
        //        checkbox.Name = ticketpriority.Name;
               
        //        if (checkbox.IsChecked)
        //            selectedPriority.Add(ticketpriority);
                
        //    }

        //    foreach (var toAdd in selectedPriority.Where(ticket => !priority.Contains(ticket)))
        //        priority.Add(toAdd);

        //    foreach (var toRemove in priority.Where(t => !selectedPriority.Contains(t)).ToList())
        //        priority.Remove(toRemove);
        //}

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
                if (tag.Id != null )
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
                    CreatedAt = DateTime.Now,
                    Slug = tag.Name.Slugify()
                };
                Database.Session.Save(newTag);
                yield return newTag;

            }
        }

        private IEnumerable<Priority> ReconsilePriority(IEnumerable<PriorityCheckbox> priorities)
        {
            foreach (var priority in priorities.Where(t => t.IsChecked))
            {
                if (priority.Id != null)
                {
                    yield return Database.Session.Load<Priority>(priority.Id);
                    continue;
                }
                var existingPriority = Database.Session.Query<Priority>().FirstOrDefault(t => t.Name == priority.Name);
                if (existingPriority != null)
                {
                    yield return existingPriority;
                    continue;
                }

                var newPriority = new Priority
                {
                    Name = priority.Name,
                    Slug = priority.Name.Slugify()
                };
                Database.Session.Save(newPriority);
                yield return newPriority;

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
                    CreatedAt = DateTime.Now,
                    User = Auth.User,
                     Slug = comment.Contents.Slugify()
                };
                Database.Session.Save(newComment);
                yield return newComment;

            }
        }

     }
}
