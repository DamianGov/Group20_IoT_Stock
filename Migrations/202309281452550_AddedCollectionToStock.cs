namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCollectionToStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StorageAreas", "Stock_Id", c => c.Int());
            CreateIndex("dbo.StorageAreas", "Stock_Id");
            AddForeignKey("dbo.StorageAreas", "Stock_Id", "dbo.Stocks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StorageAreas", "Stock_Id", "dbo.Stocks");
            DropIndex("dbo.StorageAreas", new[] { "Stock_Id" });
            DropColumn("dbo.StorageAreas", "Stock_Id");
        }
    }
}
