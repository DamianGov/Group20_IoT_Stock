namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteredExtensionTblFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExtensionRequests", "RequestId", "dbo.RequestLoanStocks");
            DropIndex("dbo.ExtensionRequests", new[] { "RequestId" });
            AddColumn("dbo.ExtensionRequests", "LoanStatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.ExtensionRequests", "LoanStatusId");
            AddForeignKey("dbo.ExtensionRequests", "LoanStatusId", "dbo.LoanStatus", "Id", cascadeDelete: true);
            DropColumn("dbo.ExtensionRequests", "RequestId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExtensionRequests", "RequestId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ExtensionRequests", "LoanStatusId", "dbo.LoanStatus");
            DropIndex("dbo.ExtensionRequests", new[] { "LoanStatusId" });
            DropColumn("dbo.ExtensionRequests", "LoanStatusId");
            CreateIndex("dbo.ExtensionRequests", "RequestId");
            AddForeignKey("dbo.ExtensionRequests", "RequestId", "dbo.RequestLoanStocks", "Id", cascadeDelete: true);
        }
    }
}
