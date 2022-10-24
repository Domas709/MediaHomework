namespace FileUploaderPrototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileTimestamps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileEntities", "InsertDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.FileEntities", "ModifiedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.FileEntities", "ContentType");
            DropTable("dbo.TestEntities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TestEntities",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        SurnameName = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
            AddColumn("dbo.FileEntities", "ContentType", c => c.String());
            DropColumn("dbo.FileEntities", "ModifiedDate");
            DropColumn("dbo.FileEntities", "InsertDate");
        }
    }
}
