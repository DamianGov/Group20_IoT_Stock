namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedWalkthroughAndWalkthroughStockTbls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Walkthroughs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        WalkthroughComplete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy, cascadeDelete: true)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.Walkthrough_Stock",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WalkthroughId = c.Int(nullable: false),
                        StockId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stocks", t => t.StockId, cascadeDelete: true)
                .ForeignKey("dbo.Walkthroughs", t => t.WalkthroughId, cascadeDelete: true)
                .Index(t => t.WalkthroughId)
                .Index(t => t.StockId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Walkthrough_Stock", "WalkthroughId", "dbo.Walkthroughs");
            DropForeignKey("dbo.Walkthrough_Stock", "StockId", "dbo.Stocks");
            DropForeignKey("dbo.Walkthroughs", "CreatedBy", "dbo.Users");
            DropIndex("dbo.Walkthrough_Stock", new[] { "StockId" });
            DropIndex("dbo.Walkthrough_Stock", new[] { "WalkthroughId" });
            DropIndex("dbo.Walkthroughs", new[] { "CreatedBy" });
            DropTable("dbo.Walkthrough_Stock");
            DropTable("dbo.Walkthroughs");
        }
    }
}
