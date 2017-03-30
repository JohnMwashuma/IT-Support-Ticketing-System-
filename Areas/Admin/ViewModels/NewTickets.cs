using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{
    public class NewTicketsIndex
    {
        public PagedData<Ticket> Tickets { get; set; }
    }
}