namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFKToUserTrackingTbl : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.UserLoginTrackings", "UserId");
            AddForeignKey("dbo.UserLoginTrackings", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLoginTrackings", "UserId", "dbo.Users");
            DropIndex("dbo.UserLoginTrackings", new[] { "UserId" });
        }
    }
}
