namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Room_Number = c.String(nullable: false, maxLength: 255),
                        Room_Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Room_Number, unique: true);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Quantity = c.Int(nullable: false),
                        QuantityBorrowed = c.Int(nullable: false),
                        LastBorrowedDate = c.DateTime(),
                        LastReturnedDate = c.DateTime(),
                        StorageAreaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StorageAreas", t => t.StorageAreaId, cascadeDelete: true)
                .Index(t => t.StorageAreaId);
            
            CreateTable(
                "dbo.StorageAreas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Area_Name = c.String(nullable: false, maxLength: 255),
                        RoomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .Index(t => t.RoomId);
            
            AddColumn("dbo.Users", "Access", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "StorageAreaId", "dbo.StorageAreas");
            DropForeignKey("dbo.StorageAreas", "RoomId", "dbo.Rooms");
            DropIndex("dbo.StorageAreas", new[] { "RoomId" });
            DropIndex("dbo.Stocks", new[] { "StorageAreaId" });
            DropIndex("dbo.Rooms", new[] { "Room_Number" });
            AlterColumn("dbo.Users", "Password", c => c.String());
            DropColumn("dbo.Users", "Access");
            DropTable("dbo.StorageAreas");
            DropTable("dbo.Stocks");
            DropTable("dbo.Rooms");
        }
    }
}
