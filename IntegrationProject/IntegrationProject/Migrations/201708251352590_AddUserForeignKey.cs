namespace IntegrationProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserForeignKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "UserID");
            AddForeignKey("dbo.AspNetUsers", "UserID", "dbo.Users", "ID");
            DropColumn("dbo.Users", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Email", c => c.String());
            DropForeignKey("dbo.AspNetUsers", "UserID", "dbo.Users");
            DropIndex("dbo.AspNetUsers", new[] { "UserID" });
            DropColumn("dbo.AspNetUsers", "UserID");
        }
    }
}
