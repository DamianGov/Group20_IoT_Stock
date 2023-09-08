namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageFieldToStockTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "ImageFile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "ImageFile");
        }
    }
}
