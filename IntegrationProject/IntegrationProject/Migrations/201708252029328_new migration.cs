namespace IntegrationProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "UserID");
            AddForeignKey("dbo.AspNetUsers", "UserID", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserID", "dbo.Users");
            DropIndex("dbo.AspNetUsers", new[] { "UserID" });
            DropColumn("dbo.AspNetUsers", "UserID");
        }
    }
}
