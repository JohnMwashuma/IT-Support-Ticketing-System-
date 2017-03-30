using System;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;


namespace ITSUPPORTTICKETSYSTEM.Models
{
    public class Role
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? DeletedAt { get; set; }

        public virtual bool IsDeleted
        {
            get { return DeletedAt != null; }
        }
    }

    public class RoleMap : ClassMapping<Role>
    {
        public RoleMap()
        {
            Table("roles");

            Id(x => x.Id, x => x.Generator(Generators.Identity));

            Property(x => x.Name, x => x.NotNullable(true));

            Property(x => x.CreatedAt, x =>
            {
                x.Column("created_at");
                x.NotNullable(false);
            });
            Property(x => x.DeletedAt, x => x.Column("deleted_at"));
        }
    }
}