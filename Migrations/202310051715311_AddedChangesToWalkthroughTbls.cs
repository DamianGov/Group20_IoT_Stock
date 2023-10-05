namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedChangesToWalkthroughTbls : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Walkthroughs", "WalkthroughDescription", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Walkthroughs", "WalkthroughDescription", c => c.String());
        }
    }
}
