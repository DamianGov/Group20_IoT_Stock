namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedQuantityToResolvedDefects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResolvedDefects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DefStockId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ResolvedNote = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DefectiveStocks", t => t.DefStockId, cascadeDelete: true)
                .Index(t => t.DefStockId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResolvedDefects", "DefStockId", "dbo.DefectiveStocks");
            DropIndex("dbo.ResolvedDefects", new[] { "DefStockId" });
            DropTable("dbo.ResolvedDefects");
        }
    }
}
