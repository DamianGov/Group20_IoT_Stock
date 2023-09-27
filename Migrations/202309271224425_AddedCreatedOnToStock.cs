namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCreatedOnToStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "CreatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "CreatedOn");
        }
    }
}
