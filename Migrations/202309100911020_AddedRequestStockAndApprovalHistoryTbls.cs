namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRequestStockAndApprovalHistoryTbls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApprovalHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        AdminId = c.Int(nullable: false),
                        ApprovalDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestStocks", t => t.RequestId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.AdminId, cascadeDelete: true)
                .Index(t => t.RequestId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.RequestStocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        StockName = c.String(nullable: false, maxLength: 255),
                        StockPrice = c.Double(nullable: false),
                        StockLink = c.String(nullable: false, maxLength: 255),
                        StockImage = c.String(),
                        RequestDate = c.DateTime(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApprovalHistories", "AdminId", "dbo.Users");
            DropForeignKey("dbo.ApprovalHistories", "RequestId", "dbo.RequestStocks");
            DropForeignKey("dbo.RequestStocks", "UserId", "dbo.Users");
            DropIndex("dbo.RequestStocks", new[] { "UserId" });
            DropIndex("dbo.ApprovalHistories", new[] { "AdminId" });
            DropIndex("dbo.ApprovalHistories", new[] { "RequestId" });
            DropTable("dbo.RequestStocks");
            DropTable("dbo.ApprovalHistories");
        }
    }
}
