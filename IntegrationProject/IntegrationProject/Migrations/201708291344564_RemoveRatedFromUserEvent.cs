namespace IntegrationProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRatedFromUserEvent : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User_Event", "Rated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User_Event", "Rated", c => c.Boolean(nullable: false));
        }
    }
}
