namespace ClassExe3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fans", "Latitude", c => c.Double(nullable: true));
            AddColumn("dbo.Fans", "Longitude", c => c.Double(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fans", "Longitude");
            DropColumn("dbo.Fans", "Latitude");
        }
    }
}
