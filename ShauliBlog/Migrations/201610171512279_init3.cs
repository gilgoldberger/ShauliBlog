namespace ClassExe3.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "City", c => c.String());
            AddColumn("dbo.Fans", "City", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fans", "City");
            DropColumn("dbo.Posts", "City");
        }
    }
}
