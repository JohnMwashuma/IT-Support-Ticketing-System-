using System.Collections.Generic;

namespace ITSUPPORTTICKETSYSTEM.Areas.Admin.ViewModels
{

    public class SidebarTag
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }

        public int TicketCount { get; private set; }

        public SidebarTag(int id, string name, string slug, int ticketcount)
        {
            Id = id;
            Name = name;
            Slug = slug;
            TicketCount = ticketcount;
        }
    }
    public class LayoutSidebar
    {

        public bool IsLoggedIn { get; set; }
        public string UserName { get; set; }

        public bool IsAdmin { get; set; }
        public IEnumerable<SidebarTag> Tags { get; set; }
         
    }
}