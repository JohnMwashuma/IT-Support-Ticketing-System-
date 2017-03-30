using FluentMigrator;

namespace ITSUPPORTTICKETSYSTEM.Migrations
{
     [Migration(9)]
    public class _009_Add_deleted_at_comments : Migration
    {
        public override void Up()
        {
           Alter.Table("comments")
       .AddColumn("deleted_at").AsDateTime().Nullable();
        }
        public override void Down()
        {
            Delete.Column("deleted_at").FromTable("comments");
        } 
    }
}