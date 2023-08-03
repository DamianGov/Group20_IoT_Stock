namespace Group20_IoT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedUserTypesToRole : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserTypes", newName: "Roles");
            RenameColumn(table: "dbo.Users", name: "UserTypeId", newName: "RoleId");
            RenameIndex(table: "dbo.Users", name: "IX_UserTypeId", newName: "IX_RoleId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Users", name: "IX_RoleId", newName: "IX_UserTypeId");
            RenameColumn(table: "dbo.Users", name: "RoleId", newName: "UserTypeId");
            RenameTable(name: "dbo.Roles", newName: "UserTypes");
        }
    }
}
