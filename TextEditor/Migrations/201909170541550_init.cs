namespace TextEditor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CountPart = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateChange = c.DateTime(nullable: false),
                        Size = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TextFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartNumber = c.Int(nullable: false),
                        TextFile = c.String(),
                        Content_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contents", t => t.Content_Id)
                .Index(t => t.Content_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TextFiles", "Content_Id", "dbo.Contents");
            DropIndex("dbo.TextFiles", new[] { "Content_Id" });
            DropTable("dbo.TextFiles");
            DropTable("dbo.Contents");
        }
    }
}
