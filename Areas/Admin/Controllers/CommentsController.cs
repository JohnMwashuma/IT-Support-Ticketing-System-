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
    [SelectedTab("comments")]
    public class CommentsController : Controller
    {
        private const int CommentsPerPage = 3;
        //
        // GET: /Admin/Comments/

        public ActionResult Index(int page = 1)
        {
            var totalCommentCount = Database.Session.Query<Comments>().Count();
            //var currentTicketPage = Database.Session.Query<Ticket>()
            //     //.Where(t => t.Status.Any(a => a.Name == "New"))
            //     //.Where(t => t.User.Username == User.Identity.Name)
            //     .OrderByDescending(c => c.CreatedAt)
            //     .Skip((page - 1)*TicketsPerPage)
            //     .Take(TicketsPerPage)
            //     .ToList();
            var currentCommentPage = Database.Session.Query<Comments>();
            if (User.IsInRole("client"))
            {
                totalCommentCount = Database.Session.Query<Ticket>().Count(t => t.User.Username == User.Identity.Name);
                // currentTicketPage = currentTicketPage.Where(t => t.User.Roles.Any(a => a.Name == "support"));
                //currentTicketPage = currentTicketPage.Where(t => t.Status.Any(a => a.Name == "New"));
                currentCommentPage = currentCommentPage.OrderByDescending(c => c.CreatedAt);
                currentCommentPage = currentCommentPage.Skip((page - 1) * CommentsPerPage);
                currentCommentPage = currentCommentPage.Take(CommentsPerPage);
                currentCommentPage.ToList();
            }
            else
            {
                currentCommentPage = currentCommentPage.OrderByDescending(c => c.CreatedAt);
                currentCommentPage = currentCommentPage.Skip((page - 1) * CommentsPerPage);
                currentCommentPage = currentCommentPage.Take(CommentsPerPage);
                currentCommentPage.ToList();
            }

            return View(new TicketCommentform()
            {
                Comments = new PagedData<Comments>(currentCommentPage, totalCommentCount, page, CommentsPerPage)
            });
        }

        public ActionResult New()
        {

            return View("Index", new TicketCommentform
            {
                IsNew = true
            });
        }

        public ActionResult Edit(int id)
        {
            var comment = Database.Session.Load<Comments>(id);
            if (comment == null)
                return HttpNotFound();
            return View("Index", new TicketCommentform
            {

                IsNew = false,
                CommentId = id,
                Contents = comment.Contents
            });
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Index(TicketCommentform index)
        {
            var comment = new Comments();

            index.IsNew = index.CommentId == null;

            if (!ModelState.IsValid)
                return View(index);



            comment.Contents = index.Contents;
            comment.CreatedAt = DateTime.UtcNow;
            comment.Slug = index.Contents.Slugify();
            comment.User = Auth.User;

            Database.Session.SaveOrUpdate(comment);

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
            var comment = Database.Session.Load<Comments>(id);
            if (comment == null)
                return HttpNotFound();

            Database.Session.Delete(comment);
            return RedirectToAction("Index");
        }

    }
}
