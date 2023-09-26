namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDescriptionForStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "Description", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "Description");
        }
    }
}
