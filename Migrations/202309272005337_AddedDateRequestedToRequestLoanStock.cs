namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDateRequestedToRequestLoanStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestLoanStocks", "DateRequested", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestLoanStocks", "DateRequested");
        }
    }
}
