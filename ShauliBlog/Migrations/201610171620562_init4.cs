namespace ClassExe3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "City", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "City");
        }
    }
}
