using System.Collections.Generic;
using System.Web.Mvc;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ITSUPPORTTICKETSYSTEM.Models
{
    public class Priority
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Slug { get; set; }
        public virtual IList<Ticket> Tickets { get; set; }
   

    public Priority()
        {
            Tickets= new List<Ticket>();
        }
   
    public class PriorityMap : ClassMapping<Priority>
    {
        public PriorityMap()
        {
            Table("ticketpriorities");

            Id(x => x.Id, x => x.Generator(Generators.Identity));

            Property(x => x.Name, x => x.NotNullable(true));
            Property(x => x.Slug, x => x.NotNullable(true));

              Bag(x => x.Tickets, x =>
                {
                    x.Key(y => y.Column("priority_id"));
                    x.Table("ticketpriority_pivot");
                }, x => x.ManyToMany(y => y.Column("ticket_id")));
        }
    }
    }
}