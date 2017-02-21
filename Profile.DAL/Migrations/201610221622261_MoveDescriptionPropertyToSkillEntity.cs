//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Profile.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveDescriptionPropertyToSkillEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Abilities", "Id", "dbo.Skills");
            DropIndex("dbo.Abilities", new[] { "Id" });
            AddColumn("dbo.Skills", "Description", c => c.String());
            DropTable("dbo.Abilities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Abilities",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Skills", "Description");
            CreateIndex("dbo.Abilities", "Id");
            AddForeignKey("dbo.Abilities", "Id", "dbo.Skills", "Id");
        }
    }
}
