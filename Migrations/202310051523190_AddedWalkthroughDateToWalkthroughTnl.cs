namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedWalkthroughDateToWalkthroughTnl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Walkthroughs", "WalkthroughDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Walkthroughs", "WalkthroughDate");
        }
    }
}
