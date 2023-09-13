namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestDecimalInStock : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stocks", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stocks", "UnitPrice", c => c.Double(nullable: false));
        }
    }
}
