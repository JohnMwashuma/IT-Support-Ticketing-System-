using FluentMigrator;

namespace ITSUPPORTTICKETSYSTEM.Migrations
{
         [Migration(4)]
    public class _004_Tag_deleted_at : Migration
    {
       public override void Up()
             {
                 Create.Column("created_at").OnTable("tags").AsDateTime().Nullable();
                 Create.Column("deleted_at").OnTable("tags").AsDateTime().Nullable();
             }
        public override void Down()
        {
            Delete.Column("created_at").FromTable("tags");
            Delete.Column("deleted_at").FromTable("tags");
        }

      }
}