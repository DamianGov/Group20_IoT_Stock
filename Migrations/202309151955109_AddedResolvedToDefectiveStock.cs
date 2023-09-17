namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedResolvedToDefectiveStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DefectiveStocks", "Resolved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DefectiveStocks", "Resolved");
        }
    }
}
