using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Infrastructure.Extensions;
using ITSUPPORTTICKETSYSTEM.Models;
using NHibernate.Linq;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,  support, client")]
    [SelectedTab("ticketcomments")]
    public class TicketCommentsController : Controller
    {
        private const int TicketsPerPage =2;

        public ActionResult Index(int page = 1)
        {
            var baseQuery = Database.Session.Query<Ticket>();
            var totalTicketCount = baseQuery.Count(t=>t.DeletedAt==null);
            if (User.IsInRole("client"))
            {
                baseQuery = Database.Session.Query<Ticket>().Where(t => t.DeletedAt == null).Where(t => t.User.Username == User.Identity.Name).OrderByDescending(t => t.CreatedAt);
                totalTicketCount = baseQuery.Count(t => t.User.Username == User.Identity.Name);
            }
            else
            {
                baseQuery = Database.Session.Query<Ticket>().Where(t => t.DeletedAt == null).OrderByDescending(t => t.CreatedAt);
                totalTicketCount = baseQuery.Count();
            }
            //var currentTicketPage = Database.Session.Query<Ticket>()
            //     //.Where(t => t.Status.Any(a => a.Name == "New"))
            //     //.Where(t => t.User.Username == User.Identity.Name)
            //     .OrderByDescending(c => c.CreatedAt)
            //     .Skip((page - 1)*TicketsPerPage)
            //     .Take(TicketsPerPage)
            //     .ToList();
            var ticketids = baseQuery.Skip((page - 1)*TicketsPerPage).Take(TicketsPerPage).Select(t => t.Id).ToArray();
            var ticketcomments =baseQuery.Where(t => ticketids.Contains(t.Id)).FetchMany(t => t.Comments).Fetch(t => t.User).ToList();
            //var currentTicketPage = Database.Session.Query<Ticket>();
            //if (User.IsInRole("user"))
            //{
            //    totalTicketCount = Database.Session.Query<Ticket>().Count(t => t.User.Username == User.Identity.Name);
            //    currentTicketPage = currentTicketPage.Where(t => t.User.Username == User.Identity.Name);
            //    // currentTicketPage = currentTicketPage.Where(t => t.User.Roles.Any(a => a.Name == "support"));
            //    //currentTicketPage = currentTicketPage.Where(t => t.Status.Any(a => a.Name == "New"));
            //    currentTicketPage = currentTicketPage.OrderByDescending(c => c.CreatedAt);
            //    currentTicketPage = currentTicketPage.Skip((page - 1) * TicketsPerPage);
            //    currentTicketPage = currentTicketPage.Take(TicketsPerPage);
            //    currentTicketPage.ToList();
            //}
            //else
            //{
            //    currentTicketPage = currentTicketPage.OrderByDescending(c => c.CreatedAt);
            //    currentTicketPage = currentTicketPage.Skip((page - 1) * TicketsPerPage);
            //    currentTicketPage = currentTicketPage.Take(TicketsPerPage);
            //    currentTicketPage.ToList();
            //}

            return View(new TicketCommentsIndex()
            {
                Tickets = new PagedData<Ticket>(ticketcomments, totalTicketCount, page, TicketsPerPage)
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
                return RedirectToRoutePermanent("Ticket", new {id = parts.Item1, slug = ticket.Slug});

            return View(new TicketsShow
            {
                Ticket = ticket
            });
        }

        private Tuple<int, string> SeperateIdAndSlug(string idAndSlug)
        {
            var matches = Regex.Match(idAndSlug, @"^(\d+)\-(.*)?$");
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
                    Comments = Database.Session.Query<Comments>().Where(t => t.User.Username == User.Identity.Name).Select(comment => new CommentCheckbox()
                    {
                        Id = comment.Id,
                        IsChecked = ticket.Comments.Contains(comment),
                        Contents = comment.Contents
                    }).Where(t => t.IsChecked == true).ToList()
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
                    Comments =Database.Session.Query<Comments>().Where(t => t.User.Username == User.Identity.Name).Select(comment => new CommentCheckbox()
                            {
                                Id = comment.Id,
                                IsChecked = ticket.Comments.Contains(comment),
                                Contents = comment.Contents
                            }).ToList()
                });

            }
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
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
            ticket.Content = form.Content;
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
                    CreatedAt = DateTime.UtcNow,
                    User = Auth.User,
                    Slug = comment.Contents.Slugify()
                };
                Database.Session.Save(newComment);
                yield return newComment;

            }
        }

    }

}
