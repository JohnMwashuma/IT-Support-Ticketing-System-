﻿using System.Web.Mvc;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName  { get { return "admin";  } }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
