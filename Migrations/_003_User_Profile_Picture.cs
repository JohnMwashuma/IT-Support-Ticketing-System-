using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace ITSUPPORTTICKETSYSTEM.Migrations
{
     [Migration(3)]
    public class _003_User_Profile_Picture : Migration
    {
         public override void Up()
         {
             Alter.Table("users")
            .AddColumn("profile_img").AsString().Nullable();
         }

         public override void Down()
         {
             Delete.Column("profile_img").FromTable("users");
         }
    }
}