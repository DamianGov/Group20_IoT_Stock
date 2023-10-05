namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDescriptionToWalkthroughTnl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Walkthroughs", "WalkthroughDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Walkthroughs", "WalkthroughDescription");
        }
    }
}
