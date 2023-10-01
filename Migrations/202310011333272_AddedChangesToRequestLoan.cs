namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedChangesToRequestLoan : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.LoanStatus", name: "AccRejBy", newName: "AccBy");
            RenameIndex(table: "dbo.LoanStatus", name: "IX_AccRejBy", newName: "IX_AccBy");
            AddColumn("dbo.LoanStatus", "AccOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.LoanStatus", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.LoanStatus", "AccRejOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LoanStatus", "AccRejOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.LoanStatus", "Status");
            DropColumn("dbo.LoanStatus", "AccOn");
            RenameIndex(table: "dbo.LoanStatus", name: "IX_AccBy", newName: "IX_AccRejBy");
            RenameColumn(table: "dbo.LoanStatus", name: "AccBy", newName: "AccRejBy");
        }
    }
}
