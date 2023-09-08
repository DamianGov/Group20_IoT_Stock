namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedChangesToStockTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "TotalQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Stocks", "QuantityOnLoan", c => c.Int(nullable: false));
            AddColumn("dbo.Stocks", "Loanable", c => c.Boolean(nullable: false));
            DropColumn("dbo.Stocks", "Quantity");
            DropColumn("dbo.Stocks", "QuantityBorrowed");
            DropColumn("dbo.Stocks", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stocks", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Stocks", "QuantityBorrowed", c => c.Int(nullable: false));
            AddColumn("dbo.Stocks", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.Stocks", "Loanable");
            DropColumn("dbo.Stocks", "QuantityOnLoan");
            DropColumn("dbo.Stocks", "TotalQuantity");
        }
    }
}
