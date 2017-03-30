using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{
    //public class Comments
    //{


    //    [DataType(DataType.MultilineText)]
    //    public string Contents { get; set; } 

    //}
    public class TicketCommentsIndex
    {
        public PagedData<User> Users { get; set; }
        public PagedData<Ticket> Tickets { get; set; }
        public IEnumerable<TicketCommentTab> TicketsData { get; set; }
       
    }

    public class TicketCommentTab
    {
        public int Id { get; private set; }
        public int TicketCount { get; private set; }
        public int NewTicketCount { get; private set; }
        public int OpenTicketCount { get; private set; }
        public int PendingTicketCount { get; private set; }
        public int ResolvedTicketCount { get; private set; }
        public TicketCommentTab(int id, int ticketcount, int newticketcount, int openticketcount, int pendingticketcount, int resolvedticketcount)
        {
            Id = id;
            TicketCount = ticketcount;
            NewTicketCount = newticketcount;
            OpenTicketCount = openticketcount;
            PendingTicketCount = pendingticketcount;
            ResolvedTicketCount = resolvedticketcount;
        }
    }

    //public class TicketsShow
    //{
    //    public Ticket Ticket { get; set; }
    //}

    //public class TicketCommentsForm
    //{
    //    public IList<StatusCheckbox> Status { get; set; }
    //    public bool IsNew { get; set; }
    //    public int? TicketId { get; set; }

    //    [Required, MaxLength(128)]
    //    public string Title { get; set; }

    //    [Required, MaxLength(128)]
    //    public string Slug { get; set; }

    //    [Required, DataType(DataType.MultilineText)]
    //    public string Content { get; set; }

    //    [DataType(DataType.MultilineText)]
    //    public string Contents { get; set; }
    //    public int? CommentId { get; set; }

    //    public IList<TagCheckbox> Tags { get; set; }
    //}
}