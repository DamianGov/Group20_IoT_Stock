namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSurnameToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FirstName", c => c.String(nullable: false, maxLength: 40));
            AddColumn("dbo.Users", "Surname", c => c.String(nullable: false, maxLength: 40));
            DropColumn("dbo.Users", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 40));
            DropColumn("dbo.Users", "Surname");
            DropColumn("dbo.Users", "FirstName");
        }
    }
}
