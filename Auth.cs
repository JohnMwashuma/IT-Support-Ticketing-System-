using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ITSUPPORTTICKETSYSTEM.Models;
using NHibernate.Linq;

namespace ITSUPPORTTICKETSYSTEM
{
    public static class Auth
    {
        private const string UserKey = "ITSUPPORTTICKETSYSTEM.Auth.UserKey";
   
    public static User User
      {
        get
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return null;

            var user = HttpContext.Current.Items[UserKey] as User;
            if (user == null)
            {
                user = Database.Session.Query<User>().FirstOrDefault(u => u.Username == HttpContext.Current.User.Identity.Name);
                if (user == null)
                    return null;

                HttpContext.Current.Items[UserKey] = user;
            }
            return user;
        }
        }

    //public static Department Department
    //{
    //    get
    //    {
    //        if (!HttpContext.Current.User.Identity.IsAuthenticated)
    //            return null;

    //        var department = HttpContext.Current.Items[UserKey] as Department;
    //        if (department == null)
    //        {
    //            department = Database.Session.Query<Department>().FirstOrDefault(u => u.Name == HttpContext.Current.User.Identity.Name);
    //            if (department == null)
    //                return null;

    //            HttpContext.Current.Items[UserKey] = department;
    //        }
    //        return Department;
    //    }
    //}
    }
}