namespace IntegrationProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRatedColumnUserEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User_Event", "Rated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User_Event", "Rated");
        }
    }
}
