//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Profile.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTraineeProfileData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artefacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        TraineeProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TraineeProfiles", t => t.TraineeProfileId)
                .Index(t => t.TraineeProfileId);
            
            CreateTable(
                "dbo.TraineeProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Email = c.String(),
                        Phone = c.String(),
                        Skype = c.String(),
                        LinkedIn = c.String(),
                        DesirablePosition = c.String(),
                        ExperienceAtITLab = c.String(),
                        ProfessionalPurposes = c.String(),
                        CurrentPosition = c.String(),
                        CurrentWork = c.String(),
                        EmploymentDuration = c.String(),
                        JobDuties = c.String(),
                        Strengths = c.String(),
                        Weaknesses = c.String(),
                        TraineeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trainees", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Educations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EducationalInstitution = c.String(),
                        TrainingPeriod = c.String(),
                        Specialization = c.String(),
                        Specialty = c.String(),
                        AdditionalInformation = c.String(),
                        TraineeProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TraineeProfiles", t => t.TraineeProfileId)
                .Index(t => t.TraineeProfileId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Level = c.String(),
                        TraineeProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TraineeProfiles", t => t.TraineeProfileId)
                .Index(t => t.TraineeProfileId);
            
            CreateTable(
                "dbo.SoftSkills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: false),
                        Name = c.String(),
                        TraineeProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TraineeProfiles", t => t.TraineeProfileId)
                .Index(t => t.TraineeProfileId);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: false),
                        Name = c.String(),
                        SpecializationId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Specializations", t => t.SpecializationId)
                .Index(t => t.SpecializationId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TraineeProfiles", "Id", "dbo.Trainees");
            DropForeignKey("dbo.Skills", "SpecializationId", "dbo.Specializations");
            DropForeignKey("dbo.SoftSkills", "TraineeProfileId", "dbo.TraineeProfiles");
            DropForeignKey("dbo.Languages", "TraineeProfileId", "dbo.TraineeProfiles");
            DropForeignKey("dbo.Educations", "TraineeProfileId", "dbo.TraineeProfiles");
            DropForeignKey("dbo.Artefacts", "TraineeProfileId", "dbo.TraineeProfiles");
            DropIndex("dbo.Skills", new[] { "SpecializationId" });
            DropIndex("dbo.SoftSkills", new[] { "TraineeProfileId" });
            DropIndex("dbo.Languages", new[] { "TraineeProfileId" });
            DropIndex("dbo.Educations", new[] { "TraineeProfileId" });
            DropIndex("dbo.TraineeProfiles", new[] { "Id" });
            DropIndex("dbo.Artefacts", new[] { "TraineeProfileId" });
            DropTable("dbo.Skills");
            DropTable("dbo.SoftSkills");
            DropTable("dbo.Languages");
            DropTable("dbo.Educations");
            DropTable("dbo.TraineeProfiles");
            DropTable("dbo.Artefacts");
        }
    }
}
