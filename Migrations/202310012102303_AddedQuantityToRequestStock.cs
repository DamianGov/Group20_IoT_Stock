namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedQuantityToRequestStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestStocks", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestStocks", "Quantity");
        }
    }
}
