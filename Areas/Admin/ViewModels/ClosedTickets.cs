using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{
    public class ClosedTicketsIndex
    {
        public PagedData<Ticket> Tickets { get; set; } 
    }
}