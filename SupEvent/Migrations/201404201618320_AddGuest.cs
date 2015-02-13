namespace _SupEvent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Guests",
                c => new
                    {
                        GuestId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Nickname = c.String(),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GuestId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Guests", "EventId", "dbo.Events");
            DropIndex("dbo.Guests", new[] { "EventId" });
            DropTable("dbo.Guests");
        }
    }
}
