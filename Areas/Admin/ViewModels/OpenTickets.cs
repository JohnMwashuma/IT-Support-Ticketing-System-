using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{
    public class OpenTicketsIndex
    {
        public PagedData<Ticket> Tickets { get; set; }
    }
}