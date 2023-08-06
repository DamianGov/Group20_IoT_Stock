namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTrackingForRelevantTbls : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Stocks", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.StorageAreas", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "CreatedBy", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CreatedBy");
            DropColumn("dbo.StorageAreas", "CreatedBy");
            DropColumn("dbo.Stocks", "CreatedBy");
            DropColumn("dbo.Rooms", "CreatedBy");
        }
    }
}
