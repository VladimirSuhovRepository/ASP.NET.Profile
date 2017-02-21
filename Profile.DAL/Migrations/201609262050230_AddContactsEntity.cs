//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Profile.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContactsEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MainSkillTraineeProfiles", "MainSkill_Id", "dbo.MainSkills");
            DropForeignKey("dbo.MainSkillTraineeProfiles", "TraineeProfile_Id", "dbo.TraineeProfiles");
            RenameTable(name: "dbo.MainSkillTraineeProfiles", newName: "TraineeProfileMainSkills");
            DropPrimaryKey("dbo.TraineeProfileMainSkills");
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        Email = c.String(),
                        Phone = c.String(),
                        Skype = c.String(),
                        LinkedIn = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            AddPrimaryKey("dbo.TraineeProfileMainSkills", new[] { "TraineeProfile_Id", "MainSkill_Id" });
            AddForeignKey("dbo.TraineeProfileMainSkills", "MainSkill_Id", "dbo.MainSkills");
            AddForeignKey("dbo.TraineeProfileMainSkills", "TraineeProfile_Id", "dbo.TraineeProfiles");
            DropColumn("dbo.TraineeProfiles", "Email");
            DropColumn("dbo.TraineeProfiles", "Phone");
            DropColumn("dbo.TraineeProfiles", "Skype");
            DropColumn("dbo.TraineeProfiles", "LinkedIn");
            DropColumn("dbo.Mentors", "ContactEmail");
            DropColumn("dbo.Mentors", "Phone");
            DropColumn("dbo.Mentors", "Skype");
            DropColumn("dbo.Mentors", "LinkedIn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Mentors", "LinkedIn", c => c.String());
            AddColumn("dbo.Mentors", "Skype", c => c.String());
            AddColumn("dbo.Mentors", "Phone", c => c.String());
            AddColumn("dbo.Mentors", "ContactEmail", c => c.String());
            AddColumn("dbo.TraineeProfiles", "LinkedIn", c => c.String());
            AddColumn("dbo.TraineeProfiles", "Skype", c => c.String());
            AddColumn("dbo.TraineeProfiles", "Phone", c => c.String());
            AddColumn("dbo.TraineeProfiles", "Email", c => c.String());
            DropForeignKey("dbo.Contacts", "UserId", "dbo.Users");
            DropIndex("dbo.Contacts", new[] { "UserId" });
            DropForeignKey("dbo.TraineeProfileMainSkills", "MainSkill_Id", "dbo.MainSkills");
            DropForeignKey("dbo.TraineeProfileMainSkills", "TraineeProfile_Id", "dbo.TraineeProfiles");
            DropPrimaryKey("dbo.TraineeProfileMainSkills");
            DropTable("dbo.Contacts");
            AddPrimaryKey("dbo.TraineeProfileMainSkills", new[] { "MainSkill_Id", "TraineeProfile_Id" });
            RenameTable(name: "dbo.TraineeProfileMainSkills", newName: "MainSkillTraineeProfiles");
            AddForeignKey("dbo.MainSkillTraineeProfiles", "MainSkill_Id", "dbo.MainSkills");
            AddForeignKey("dbo.MainSkillTraineeProfiles", "TraineeProfile_Id", "dbo.TraineeProfiles");
        }
    }
}
