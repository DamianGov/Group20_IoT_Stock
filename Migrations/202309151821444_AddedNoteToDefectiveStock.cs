namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNoteToDefectiveStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DefectiveStocks", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DefectiveStocks", "Note");
        }
    }
}
