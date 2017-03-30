using FluentMigrator;

namespace ITSUPPORTTICKETSYSTEM.Migrations
{
     [Migration(7)]
    public class _007_Role_add_created_at : Migration
    {
        public override void Up()
        {
            Alter.Table("roles")
         .AddColumn("created_at").AsDateTime().Nullable();
            Alter.Table("roles")
       .AddColumn("deleted_at").AsDateTime().Nullable();
        }
        public override void Down()
        {
            Delete.Column("created_at").FromTable("roles");
            Delete.Column("deleted_at").FromTable("roles");
        }
    }
}