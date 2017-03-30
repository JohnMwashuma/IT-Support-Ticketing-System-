using System;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ITSUPPORTTICKETSYSTEM.Models
{
    public class Comments
    {
        public virtual int Id { get; set; }
        public virtual User User { get; set; }
        public virtual string Slug { get; set; }
        public virtual string Contents { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        public virtual IList<Ticket> Tickets { get; set; }

        public virtual DateTime? DeletedAt { get; set; }

        public virtual bool IsDeleted
        {
            get { return DeletedAt != null; }
        }

        public Comments()
        {
            Tickets = new List<Ticket>();
        }

        public class CommentsMap: ClassMapping<Comments>
        {
            public CommentsMap()
            {
                Table("comments");

                Id(x => x.Id, x => x.Generator(Generators.Identity));

                Property(x => x.Slug, x => x.NotNullable(true));
                Property(x => x.Contents, x => x.NotNullable(true));

                Property(x => x.CreatedAt, x =>
                {
                    x.Column("created_at");
                    x.NotNullable(true);
                });

                Property(x => x.DeletedAt, x => x.Column("deleted_at"));

                ManyToOne(x => x.User, x =>
                {
                    x.Column("user_id");
                    x.NotNullable(true);
                });

                //ManyToOne(x => x.Tickets, x =>
                //{
                //    x.Column("ticket_id");
                //    x.NotNullable(true);
                //});
                //HasMany(m => m.Tickets).KeyColumns.Add("ticket_id");
                Bag(x => x.Tickets, x =>
                {
                    x.Table("ticket_comments");
                    x.Key(y => y.Column("comment_id"));
                }, x => x.ManyToMany(y => y.Column("ticket_id")));
            }
        }
    }
}