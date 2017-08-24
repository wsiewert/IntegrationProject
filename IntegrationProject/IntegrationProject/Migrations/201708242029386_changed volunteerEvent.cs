namespace IntegrationProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedvolunteerEvent : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VolunteerEvents", "HostID", c => c.String());
            DropColumn("dbo.VolunteerEvents", "AllDay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VolunteerEvents", "AllDay", c => c.Boolean(nullable: false));
            AlterColumn("dbo.VolunteerEvents", "HostID", c => c.Int(nullable: false));
        }
    }
}
