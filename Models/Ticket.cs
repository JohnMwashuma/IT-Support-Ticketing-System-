using System;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ITSUPPORTTICKETSYSTEM.Models
{
    public  class Ticket
    {
        public virtual int Id { get; set; }

        public virtual User User { get; set; }

        //public virtual User Department { get; set; }

        //public virtual Comments Comments { get; set; }

        public virtual string Title { get; set; }
        public virtual string Slug { get; set; }

        public virtual string Content { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual DateTime? DeletedAt { get; set; }
     
    

        public virtual IList<TicketStatus> Status { get; set; }

        public virtual IList<Priority> Priorities { get; set; }

         public Ticket()
        {
            Priorities = new List<Priority>();
           Status = new List<TicketStatus>();
           Comments = new List<Comments>();
           Tags = new List<Tag>();
        }


        public virtual IList<Tag> Tags { get; set; }

        public virtual IList<Comments> Comments { get; set; }

        public virtual bool IsDeleted
        {
            get { return DeletedAt != null; }
        }
    }

    public class TicketMap:ClassMapping<Ticket>
    {
        public TicketMap()
        {
            Table("tickets");

            Id(x => x.Id, x => x.Generator(Generators.Identity));

           
           
            Property(x => x.Title, x => x.NotNullable(true));
            Property(x => x.Slug, x => x.NotNullable(true));
            Property(x => x.Content, x => x.NotNullable(true));
          
            

            Property(x => x.CreatedAt, x =>
            {
                x.Column("created_at");
                x.NotNullable(true);
            });
            Property(x => x.UpdatedAt, x => x.Column("updated_at"));
            Property(x => x.DeletedAt, x => x.Column("deleted_at"));

            ManyToOne(x => x.User, x =>
            {
                x.Column("user_id");
                x.NotNullable(true);
            });
            //ManyToOne(x => x.Department, x =>
            //{
            //    x.Column("department_id");
            //    x.NotNullable(true);
            //});

            //ManyToOne(x => x.Comments, x =>
            //{
            //    x.Column("comment_id");
            //    x.NotNullable(true);
            //});

            Bag(x => x.Tags, x =>
            {
                x.Key(y => y.Column("ticket_id"));
                x.Table("ticket_tags");
            }, x => x.ManyToMany(y => y.Column("tag_id")));

            Bag(x => x.Status, x =>
            {
              x.Table("ticket_status");
              x.Key(k => k.Column("ticket_id"));
            }, x => x.ManyToMany(k => k.Column("status_id")));

            Bag(x => x.Priorities, x =>
            {
                x.Table("ticketpriority_pivot");
                x.Key(k => k.Column("ticket_id"));
            }, x => x.ManyToMany(k => k.Column("priority_id")));

            Bag(x => x.Comments, x =>
            {
                x.Table("ticket_comments");
                x.Key(y => y.Column("ticket_id"));
            }, x => x.ManyToMany(y => y.Column("comment_id")));

        }
    }
}
