namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNotifyToUsersTBL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Notify", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Notify");
        }
    }
}
