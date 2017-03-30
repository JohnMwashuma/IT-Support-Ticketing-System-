using FluentMigrator;

namespace ITSUPPORTTICKETSYSTEM.Migrations
{
    [Migration(8)]
    public class _008_Department_add_created_at : Migration
    {
        public override void Up()
        {
            Alter.Table("department")
         .AddColumn("created_at").AsDateTime().Nullable();
            Alter.Table("department")
       .AddColumn("deleted_at").AsDateTime().Nullable();
        }
        public override void Down()
        {
            Delete.Column("created_at").FromTable("department");
            Delete.Column("deleted_at").FromTable("department");
        } 
    }
}