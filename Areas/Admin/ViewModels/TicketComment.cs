using System.ComponentModel.DataAnnotations;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{
    public class TicketComment
    {
         
    }
    public class TicketCommentform
    {
        public PagedData<Comments> Comments { get; set; }
        public bool IsNew { get; set; }

        public int? CommentId { get; set; }

        [Required, MaxLength(128)]
        public string Contents { get; set; }
    }
}