namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNoteToLoanStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoanStatus", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoanStatus", "Note");
        }
    }
}
