namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDefectToDiscrepancy : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DefectiveStocks", newName: "StockDiscrepancies");
            AddColumn("dbo.StockDiscrepancies", "DiscrepancyCategory", c => c.Int(nullable: false));
            AddColumn("dbo.StockDiscrepancies", "DiscrepancyFound", c => c.DateTime(nullable: false));
            DropColumn("dbo.StockDiscrepancies", "DefectiveCategory");
            DropColumn("dbo.StockDiscrepancies", "DefectFound");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockDiscrepancies", "DefectFound", c => c.DateTime(nullable: false));
            AddColumn("dbo.StockDiscrepancies", "DefectiveCategory", c => c.Int(nullable: false));
            DropColumn("dbo.StockDiscrepancies", "DiscrepancyFound");
            DropColumn("dbo.StockDiscrepancies", "DiscrepancyCategory");
            RenameTable(name: "dbo.StockDiscrepancies", newName: "DefectiveStocks");
        }
    }
}
