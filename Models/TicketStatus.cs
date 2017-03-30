using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ITSUPPORTTICKETSYSTEM.Models
{
    public class TicketStatus
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; } 
    }

    public class TicketStatusMap : ClassMapping<TicketStatus>
    {
        public TicketStatusMap()
        {
            Table("status");

            Id(x => x.Id, x => x.Generator(Generators.Identity));

            Property(x => x.Name, x => x.NotNullable(true));
        }
    }
}