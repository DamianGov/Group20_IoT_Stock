namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRequestExtensionToLoanStatusTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoanStatus", "RequestExtension", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoanStatus", "RequestExtension");
        }
    }
}
