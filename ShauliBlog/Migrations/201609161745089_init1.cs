namespace ClassExe3.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            /*CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        PostID = c.Int(nullable: false),
                        Title = c.String(),
                        Author = c.String(),
                        Website = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        Website = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                        Content = c.String(),
                        Image = c.String(),
                        Video = c.String(),
                    })
                .PrimaryKey(t => t.PostID);
            
            CreateTable(
                "dbo.Fans",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        gender = c.Int(nullable: false),
                        birthDate = c.DateTime(nullable: false),
                        seniority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);*/
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "PostID" });
            DropTable("dbo.Fans");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
        }
    }
}
