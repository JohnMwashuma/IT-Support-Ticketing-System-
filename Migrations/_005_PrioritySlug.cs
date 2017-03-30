using FluentMigrator;

namespace ITSUPPORTTICKETSYSTEM.Migrations
{
     [Migration(5)]
    public class _005_PrioritySlug : Migration
    {
        public override void Up()
        {
            Create.Column("slug").OnTable("priority").AsString(128).Nullable();
        }
        public override void Down()
        {
            Delete.Column("slug").FromTable("priority");
        }
    }
}