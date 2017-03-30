using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ITSUPPORTTICKETSYSTEM.Areas.Admin.Controllers;

namespace ITSUPPORTTICKETSYSTEM
{

    public class RouteConfig
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            var namespaces = new[] { typeof(TicketCommentsController).Namespace };
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Comment", "admin/comment/{id}-{slug}", new { Controller = "Tickets", Action = "Comment" });

            routes.MapRoute("TagForRealThisTime", "admin/Dashboard/tag/{idandslug}", new {Area="Admin", Controller = "Dashboard", Action = "Tag" }, namespaces);
            routes.MapRoute("Tag", "admin/Dashboard/tag/{id}-{slug}", new { Area = "Admin", Controller = "Dashboard", Action = "Tag" }, namespaces);

            routes.MapRoute("TicketForRealThisTime", "admin/Dashboard/show/{idandslug}", new { Controller = "Dashboard", Action = "Show" }, namespaces);
            routes.MapRoute("Ticket", "admin/Dashboard/show/{id}-{slug}", new { Controller = "Dashboard", Action = "Show" }, namespaces);

            routes.MapRoute("Login", "login", new { Controller = "SupportHome", Action = "Login" }, namespaces);
            routes.MapRoute("Logout", "logout", new { Controller = "SupportHome", Action = "logout" }, namespaces);
            routes.MapRoute("Dashboard", "admin/Dashboard", new { Action = "index", id = UrlParameter.Optional },namespaces);
            routes.MapRoute("Home", "", new { controller = "SupportHome", Action = "Login" });

            routes.MapRoute("Sidebar", "", new { controller = "Layout", Action = "Sidebar" }, namespaces);
          
        }
    }
}