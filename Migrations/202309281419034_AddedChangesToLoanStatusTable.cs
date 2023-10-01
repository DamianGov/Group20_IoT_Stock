namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedChangesToLoanStatusTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoanStatus", "AccRejOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.LoanStatus", "Note");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LoanStatus", "Note", c => c.String());
            DropColumn("dbo.LoanStatus", "AccRejOn");
        }
    }
}
