﻿namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedExtensionSubmittedToExtensionRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExtensionRequests", "ExtensionSubmitted", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExtensionRequests", "ExtensionSubmitted");
        }
    }
}
