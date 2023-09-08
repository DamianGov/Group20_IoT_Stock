namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageFieldRemRequiredToStockTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stocks", "ImageFile", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stocks", "ImageFile", c => c.String(nullable: false));
        }
    }
}
