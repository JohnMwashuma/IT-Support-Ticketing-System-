using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{
    public class Tags
    {
        
    }

    public class Tagsform
    {
        public PagedData<Tag> Tagz { get; set; }
        public bool IsNew { get; set; }

        public int? TagId { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }
    }

}