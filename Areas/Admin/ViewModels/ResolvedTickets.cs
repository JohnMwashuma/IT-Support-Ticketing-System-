using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{



    public class ResolvedTicketsIndex
    {
        public PagedData<Ticket> Tickets { get; set; }
    }

   
}