namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUniNumToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UniNum", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "UniNum");
        }
    }
}
