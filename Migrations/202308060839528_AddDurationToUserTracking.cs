namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDurationToUserTracking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLoginTrackings", "Duration", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLoginTrackings", "Duration");
        }
    }
}
