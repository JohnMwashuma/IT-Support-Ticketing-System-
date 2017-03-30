using System;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ITSUPPORTTICKETSYSTEM.Models
{
    public class Tag
    {
        public virtual int Id { get; set; }
        public virtual string Slug { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? DeletedAt { get; set; }

        public virtual IList<Ticket> Tickets { get; set; }

        public virtual bool IsDeleted
        {
            get { return DeletedAt != null; }
        }

        public Tag()
        {
            Tickets= new List<Ticket>();
        }

        public class TagMap:ClassMapping<Tag>
        {
            public TagMap()
            {
                Table("tags");

                Id(x => x.Id, x => x.Generator(Generators.Identity));

                Property(x => x.Slug, x => x.NotNullable(true));
                Property(x => x.Name, x => x.NotNullable(true));

                Property(x => x.CreatedAt, x =>
                {
                    x.Column("created_at");
                    x.NotNullable(false);
                });
                Property(x => x.DeletedAt, x => x.Column("deleted_at"));

                Bag(x => x.Tickets, x =>
                {
                    x.Key(y => y.Column("tag_id"));
                    x.Table("ticket_tags");
                }, x => x.ManyToMany(y => y.Column("ticket_id")));
            }
        }
    }
}
    