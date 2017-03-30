using System.ComponentModel.DataAnnotations;
using ITSUPPORTTICKETSYSTEM.Infrastructure;
using ITSUPPORTTICKETSYSTEM.Models;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{
    public class Departments
    {

    }

    public class Departmentsform
    {
        public PagedData<Department> Departments { get; set; }
        public bool IsNew { get; set; }

        public int? DepartmentId { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }
    }
}