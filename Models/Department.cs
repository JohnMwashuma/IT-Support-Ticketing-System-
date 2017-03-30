using System;
using System.Collections.Generic;
using ITSUPPORTTICKETSYSTEM.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ITSUPPORTTICKETSYSTEM.Models
{
    public class Department
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual IList<User> Users { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? DeletedAt { get; set; }

        public virtual bool IsDeleted
        {
            get { return DeletedAt != null; }
        }
        public Department()
        {
            Users = new List<User>();
        }
    }
    public class DepartmentMap : ClassMapping<Department>
    {
        public DepartmentMap()
        {
            Table("department");

            Id(x => x.Id, x => x.Generator(Generators.Identity));

            Property(x => x.Name, x => x.NotNullable(true));

            Bag(x => x.Users, x =>
            {
                x.Table("department_users");
                x.Key(k => k.Column("department_id"));
            }, x => x.ManyToMany(k => k.Column("user_id")));

            Property(x => x.CreatedAt, x =>
            {
                x.Column("created_at");
                x.NotNullable(false);
            });
            Property(x => x.DeletedAt, x => x.Column("deleted_at"));
            //Bag(x => x.Tickets, x =>
            //{
            //    x.Table("department_users");
            //    x.Key(k => k.Column("user_id"));
            //}, x => x.ManyToMany(k => k.Column("department_id")));
        }
    }
}