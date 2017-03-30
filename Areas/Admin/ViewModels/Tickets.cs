using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{
    public class PriorityCheckbox
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
    public class TicketsTag
    {
        public Tag Tag { get; set; }
        public PagedData<Ticket> Tickets { get; set; }
    }
    public class CommentCheckbox
    {
        public int? Id { get; set; }
        public string Contents { get; set; }
        public bool IsChecked { get; set; }
    }

   public class StatusCheckbox
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
    public class TagCheckbox
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }

    public class TicketsIndex
    {
        public IList<StatusCheckbox> Status { get; set; }
        public bool IsNew { get; set; }
        public int? TicketId { get; set; }

        [Required, MaxLength(128)]
        public string Title { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public PagedData<Ticket> Tickets { get; set; }
        //public PagedData<User> Users { get; set; }
    }

    public class TicketsShow
    {
        public Ticket Ticket { get; set; }
    }

    public class TicketsForm
    {
        public IList<PriorityCheckbox> Priorities { get; set; }
        public IList<StatusCheckbox> Status { get; set; }

        public bool IsNew { get; set; }
        public int? TicketId { get; set; }
        public int? CommentId { get; set; }
       
        [Required, MaxLength(128)]
        public string Title { get; set; }

        [Required, MaxLength(128)]
        public string Slug { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public IList<CommentCheckbox> Comments{ get; set; }

        public IList<TagCheckbox> Tags { get; set; }
        

    }
}