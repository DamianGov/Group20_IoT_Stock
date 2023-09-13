namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedActiveAndUnitPriceToStorageAndStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "UnitPrice", c => c.Double(nullable: false));
            AddColumn("dbo.StorageAreas", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StorageAreas", "Active");
            DropColumn("dbo.Stocks", "UnitPrice");
        }
    }
}
