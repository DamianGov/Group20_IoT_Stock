namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedRoomTbl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rooms", "Room_Description", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rooms", "Room_Description", c => c.String(nullable: false));
        }
    }
}
