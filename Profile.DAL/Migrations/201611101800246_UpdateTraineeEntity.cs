//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Profile.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTraineeEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Trainees", "SpecializationId", "dbo.Specializations");
            DropIndex("dbo.Trainees", new[] { "SpecializationId" });
            AlterColumn("dbo.Trainees", "SpecializationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Trainees", "SpecializationId");
            AddForeignKey("dbo.Trainees", "SpecializationId", "dbo.Specializations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trainees", "SpecializationId", "dbo.Specializations");
            DropIndex("dbo.Trainees", new[] { "SpecializationId" });
            AlterColumn("dbo.Trainees", "SpecializationId", c => c.Int());
            CreateIndex("dbo.Trainees", "SpecializationId");
            AddForeignKey("dbo.Trainees", "SpecializationId", "dbo.Specializations", "Id");
        }
    }
}
