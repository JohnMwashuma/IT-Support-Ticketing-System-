using System.Data;
using FluentMigrator;

namespace ITSUPPORTTICKETSYSTEM.Migrations
{

    [Migration(2)]
    public class _002_TicketsAndTags : Migration
    {
        public override void Up()
        {
            
            Create.Table("tickets")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id")
               .WithColumn("title").AsString(128)
                .WithColumn("slug").AsString(128)
                .WithColumn("content").AsCustom("Text")
                .WithColumn("created_at").AsDateTime()
                .WithColumn("updated_at").AsDateTime().Nullable()
                .WithColumn("deleted_at").AsDateTime().Nullable();

            Create.Table("comments")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id")
                .WithColumn("slug").AsString(128)
                .WithColumn("contents").AsString(128)
                .WithColumn("created_at").AsDateTime();
            

            Create.Table("status")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString(128);

            Create.Table("priority")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString(128);

            Create.Table("tags")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("slug").AsString(128)
                .WithColumn("name").AsString(128);

            Create.Table("ticket_comments")
                .WithColumn("comment_id").AsInt32().ForeignKey("comments", "id").OnDelete(Rule.Cascade)
                .WithColumn("ticket_id ").AsInt32().ForeignKey("tickets", "id").OnDelete(Rule.Cascade);

            Create.Table("ticket_status")
               .WithColumn("status_id").AsInt32().ForeignKey("status", "id").OnDelete(Rule.Cascade)
               .WithColumn("ticket_id ").AsInt32().ForeignKey("tickets", "id").OnDelete(Rule.Cascade);

            Create.Table("ticket_priority")
               .WithColumn("priority-id").AsInt32().ForeignKey("priority", "id").OnDelete(Rule.Cascade)
               .WithColumn("ticket_id").AsInt32().ForeignKey("tickets", "id").OnDelete(Rule.Cascade);

            Create.Table("ticket_tags")
                .WithColumn("tag_id").AsInt32().ForeignKey("tags", "id").OnDelete(Rule.Cascade)
                .WithColumn("ticket_id").AsInt32().ForeignKey("tickets", "id").OnDelete(Rule.Cascade);

          
        }

        public override void Down()
        {
            Delete.Table("ticket_comments");
            Delete.Table("ticket_status");
            Delete.Table("ticket_priority");
            Delete.Table("ticket_tags");
            Delete.Table("tickets");
            Delete.Table("comments");
            Delete.Table("status");
            Delete.Table("priority");
            Delete.Table("tags");
        }
    }
}