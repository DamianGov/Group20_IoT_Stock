namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedResolvedDateToDefectiveStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DefectiveStocks", "ResolvedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DefectiveStocks", "ResolvedDate");
        }
    }
}
