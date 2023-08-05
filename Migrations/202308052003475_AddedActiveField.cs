namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedActiveField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Stocks", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "Active");
            DropColumn("dbo.Rooms", "Active");
        }
    }
}
