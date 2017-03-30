using System.Data;
using FluentMigrator;

namespace ITSUPPORTTICKETSYSTEM.Migrations
{
   
    public class _003_TicketsComments : Migration
    {

        public override void Up()
        {
           
            Create.Table("ticket_comments")
                .WithColumn("comment_id").AsInt32().ForeignKey("comments", "id").OnDelete(Rule.Cascade)
                .WithColumn("ticket_id").AsInt32().ForeignKey("tickets", "id").OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("ticket_comments");
           
        }

    }
}