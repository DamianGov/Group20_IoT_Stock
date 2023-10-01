namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedExtensionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExtensionRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        Reason = c.String(),
                        Status = c.Int(nullable: false),
                        StatusBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestLoanStocks", t => t.RequestId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.StatusBy)
                .Index(t => t.RequestId)
                .Index(t => t.StatusBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExtensionRequests", "StatusBy", "dbo.Users");
            DropForeignKey("dbo.ExtensionRequests", "RequestId", "dbo.RequestLoanStocks");
            DropIndex("dbo.ExtensionRequests", new[] { "StatusBy" });
            DropIndex("dbo.ExtensionRequests", new[] { "RequestId" });
            DropTable("dbo.ExtensionRequests");
        }
    }
}
