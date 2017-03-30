using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ITSUPPORTTICKETSYSTEM.Migrations
{
    [Migration(1)]
    public class _001_UserAndRoles : Migration
    {

        public override void Up()
        {
            Create.Table("users")
               .WithColumn("id").AsInt32().Identity().PrimaryKey()
               .WithColumn("username").AsString(128)
               .WithColumn("email").AsString(256)
               .WithColumn("password_hash").AsString(128);

            Create.Table("roles")
               .WithColumn("id").AsInt32().Identity().PrimaryKey()
               .WithColumn("name").AsString(128);

            Create.Table("department")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128);

            Create.Table("role_users")
               .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
               .WithColumn("role_id").AsInt32().ForeignKey("roles", "id").OnDelete(Rule.Cascade);

            Create.Table("department_users")
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("department_id").AsInt32().ForeignKey("department", "id").OnDelete(Rule.Cascade);
        }
        public override void Down()
        {
            Delete.Table("role_users");
            Delete.Table("department_users");
            Delete.Table("roles");
            Delete.Table("department");
            Delete.Table("users");
        }
    }
}