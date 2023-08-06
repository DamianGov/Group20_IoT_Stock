namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserTracking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLoginTrackings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        UserLoginDateTime = c.DateTime(),
                        UserLogoutDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserLoginTrackings");
        }
    }
}
