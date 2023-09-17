namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUsedRememberMeToUserTracking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLoginTrackings", "UsedRememberMe", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLoginTrackings", "UsedRememberMe");
        }
    }
}
