namespace _SupEvent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "EventTime");
        }
    }
}
