using ITSUPPORTTICKETSYSTEM.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ITSUPPORTTICKETSYSTEM.Infrastructure;


namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{
    public class DepartmentCheckbox
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }

    }
    public class RoleCheckbox
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }

    }
    public class UsersIndex
    {
        public PagedData<User> Users { get; set; }
        
       
    }
    public class UsersNew
    {
        public IList<RoleCheckbox> Roles { get; set; }

        public IList<DepartmentCheckbox> Departments { get; set; }

        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

  
        [DataType(DataType.Upload)]
        public string ProfilePicture { get; set; }
    }
    public class UsersEdit
    {
        public IList<RoleCheckbox> Roles { get; set; }

        public IList<DepartmentCheckbox> Departments { get; set; }

        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Upload)]
        public string ProfilePicture { get; set; }
    }
    public class UsersResetPassword
    {
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

    }
}