using System.ComponentModel.DataAnnotations;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{
    public class Roles
    {
         
    }

    public class Rolesform
    {
        public PagedData<Role> Roles { get; set; }
        public bool IsNew { get; set; }

        public int? RoleId { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }
    }
}