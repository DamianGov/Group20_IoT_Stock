namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedStatusByFromExtensionRequest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExtensionRequests", "StatusBy", "dbo.Users");
            DropIndex("dbo.ExtensionRequests", new[] { "StatusBy" });
            DropColumn("dbo.ExtensionRequests", "StatusBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExtensionRequests", "StatusBy", c => c.Int(nullable: false));
            CreateIndex("dbo.ExtensionRequests", "StatusBy");
            AddForeignKey("dbo.ExtensionRequests", "StatusBy", "dbo.Users", "Id");
        }
    }
}
