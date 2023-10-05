namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedStockAllWalkthrough : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Walkthroughs", "StockStillAllocated", c => c.Boolean(nullable: false));
            DropColumn("dbo.Walkthroughs", "WalkthroughComplete");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Walkthroughs", "WalkthroughComplete", c => c.Boolean(nullable: false));
            DropColumn("dbo.Walkthroughs", "StockStillAllocated");
        }
    }
}
