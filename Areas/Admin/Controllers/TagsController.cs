using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Infrastructure.Extensions;
using ITSUPPORTTICKETSYSTEM.Models;
using NHibernate.Linq;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,  support, client")]
    [SelectedTab("tags")]
    public class TagsController : Controller
    {
        private const int TagsPerPage = 3;
        //
        // GET: /Admin/Tags/

        public ActionResult Index(int page = 1)
        {

            var totalTagCount = Database.Session.Query<Tag>().Count();
            //var currentTicketPage = Database.Session.Query<Ticket>()
            //     //.Where(t => t.Status.Any(a => a.Name == "New"))
            //     //.Where(t => t.User.Username == User.Identity.Name)
            //     .OrderByDescending(c => c.CreatedAt)
            //     .Skip((page - 1)*TicketsPerPage)
            //     .Take(TicketsPerPage)
            //     .ToList();
            var currentTagPage = Database.Session.Query<Tag>();
            if (User.IsInRole("client"))
            {
                totalTagCount = Database.Session.Query<Ticket>().Count(t => t.User.Username == User.Identity.Name);
                // currentTicketPage = currentTicketPage.Where(t => t.User.Roles.Any(a => a.Name == "support"));
                //currentTicketPage = currentTicketPage.Where(t => t.Status.Any(a => a.Name == "New"));
                currentTagPage = currentTagPage.OrderByDescending(c => c.CreatedAt);
                currentTagPage = currentTagPage.Skip((page - 1) * TagsPerPage);
                currentTagPage = currentTagPage.Take(TagsPerPage);
                currentTagPage.ToList();
            }
            else
            {
                currentTagPage = currentTagPage.OrderByDescending(c => c.CreatedAt);
                currentTagPage = currentTagPage.Skip((page - 1) * TagsPerPage);
                currentTagPage = currentTagPage.Take(TagsPerPage);
                currentTagPage.ToList();
            }

            return View(new Tagsform()
            {
                Tagz = new PagedData<Tag>(currentTagPage, totalTagCount, page, TagsPerPage)
            });
        }

        public ActionResult New()
        {

            return View("Index", new Tagsform
            {
                IsNew = true
            });
        }

        public ActionResult Edit(int id)
        {
            var tag = Database.Session.Load<Tag>(id);
            if (tag == null)
                return HttpNotFound();
            return View("Index", new Tagsform
            {

                IsNew = false,
                TagId = id,
                Name = tag.Name
            });
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Index(Tagsform index)
        {
            var tag = new Tag();

            index.IsNew = index.TagId == null;

            if (!ModelState.IsValid)
                return View(index);


         
            tag.Name = index.Name;
            tag.CreatedAt = DateTime.UtcNow;
            tag.Slug = index.Name.Slugify();

            Database.Session.SaveOrUpdate(tag);

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
            var tag = Database.Session.Load<Tag>(id);
            if (tag == null)
                return HttpNotFound();

            Database.Session.Delete(tag);
            return RedirectToAction("Index");
        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult Restore(int id)
        //{
        //    var ticket = Database.Session.Load<Ticket>(id);
        //    if (ticket == null)
        //        return HttpNotFound();

        //    ticket.DeletedAt = null;
        //    Database.Session.Update(ticket);
        //    return RedirectToAction("Index");
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
