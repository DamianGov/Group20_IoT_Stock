namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDescriptionForStock : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stocks", "Description", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stocks", "Description", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
