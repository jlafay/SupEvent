namespace _SupEvent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Friend : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "User_UserId", "dbo.Users");
            DropIndex("dbo.Users", new[] { "User_UserId" });
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        FriendId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Nickname = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FriendId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            DropColumn("dbo.Users", "User_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "User_UserId", c => c.Int());
            DropForeignKey("dbo.Friends", "UserId", "dbo.Users");
            DropIndex("dbo.Friends", new[] { "UserId" });
            DropTable("dbo.Friends");
            CreateIndex("dbo.Users", "User_UserId");
            AddForeignKey("dbo.Users", "User_UserId", "dbo.Users", "UserId");
        }
    }
}
