namespace _SupEvent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGuest : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Events", "Guests");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "Guests", c => c.String());
        }
    }
}
