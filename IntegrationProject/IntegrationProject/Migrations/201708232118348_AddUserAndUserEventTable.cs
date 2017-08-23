namespace IntegrationProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAndUserEventTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Description = c.String(),
                        VolunteerUpVotes = c.Int(nullable: false),
                        VolunteerDownVotes = c.Int(nullable: false),
                        EventUpVotes = c.Int(nullable: false),
                        EventDownVotes = c.Int(nullable: false),
                        NoShowCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.User_Event",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        VolunteerEventID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.VolunteerEvents", t => t.VolunteerEventID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.VolunteerEventID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User_Event", "VolunteerEventID", "dbo.VolunteerEvents");
            DropForeignKey("dbo.User_Event", "UserID", "dbo.Users");
            DropIndex("dbo.User_Event", new[] { "VolunteerEventID" });
            DropIndex("dbo.User_Event", new[] { "UserID" });
            DropTable("dbo.User_Event");
            DropTable("dbo.Users");
        }
    }
}
