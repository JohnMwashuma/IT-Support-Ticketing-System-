using System.Data;
using FluentMigrator;

namespace ITSUPPORTTICKETSYSTEM.Migrations
{
     [Migration(6)]
    public class _006_TicketPriority: Migration
    {
        public override void Up()
        {
            Create.Table("ticketpriorities")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("slug").AsString(128)
               .WithColumn("name").AsString(128);

            Create.Table("ticketpriority_pivot")
                .WithColumn("priority_id").AsInt32().ForeignKey("ticketpriorities", "id").OnDelete(Rule.Cascade)
                .WithColumn("ticket_id").AsInt32().ForeignKey("tickets", "id").OnDelete(Rule.Cascade);


        }

        public override void Down()
        {
            Delete.Table("ticketpriority_pivot");
            Delete.Table("ticketpriorities");
        }
    }
}