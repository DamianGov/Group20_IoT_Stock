namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRequestLoanStockAndLoanStatusTbls : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApprovalHistories", "RequestId", "dbo.RequestStocks");
            DropForeignKey("dbo.ApprovalHistories", "AdminId", "dbo.Users");
            DropForeignKey("dbo.ResolvedDefects", "DefStockId", "dbo.StockDiscrepancies");
            DropIndex("dbo.ApprovalHistories", new[] { "RequestId" });
            DropIndex("dbo.ApprovalHistories", new[] { "AdminId" });
            DropIndex("dbo.ResolvedDefects", new[] { "DefStockId" });
            CreateTable(
                "dbo.LoanStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        Note = c.String(),
                        AccRejBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestLoanStocks", t => t.RequestId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.AccRejBy)
                .Index(t => t.RequestId)
                .Index(t => t.AccRejBy);
            
            CreateTable(
                "dbo.RequestLoanStocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StockId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stocks", t => t.StockId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.StockId)
                .Index(t => t.UserId);
            
            DropTable("dbo.ApprovalHistories");
            DropTable("dbo.ResolvedDefects");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApprovalHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        AdminId = c.Int(nullable: false),
                        ApprovalDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.LoanStatus", "AccRejBy", "dbo.Users");
            DropForeignKey("dbo.LoanStatus", "RequestId", "dbo.RequestLoanStocks");
            DropForeignKey("dbo.RequestLoanStocks", "UserId", "dbo.Users");
            DropForeignKey("dbo.RequestLoanStocks", "StockId", "dbo.Stocks");
            DropIndex("dbo.RequestLoanStocks", new[] { "UserId" });
            DropIndex("dbo.RequestLoanStocks", new[] { "StockId" });
            DropIndex("dbo.LoanStatus", new[] { "AccRejBy" });
            DropIndex("dbo.LoanStatus", new[] { "RequestId" });
            DropTable("dbo.RequestLoanStocks");
            DropTable("dbo.LoanStatus");
            CreateIndex("dbo.ResolvedDefects", "DefStockId");
            CreateIndex("dbo.ApprovalHistories", "AdminId");
            CreateIndex("dbo.ApprovalHistories", "RequestId");
            AddForeignKey("dbo.ResolvedDefects", "DefStockId", "dbo.StockDiscrepancies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApprovalHistories", "AdminId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApprovalHistories", "RequestId", "dbo.RequestStocks", "Id", cascadeDelete: true);
        }
    }
}
