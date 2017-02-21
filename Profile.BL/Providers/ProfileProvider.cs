using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class ProfileProvider : IProfileProvider
    {
        private const double MentorReviewWeightCoefficient = 0.4;
        private const double ScrumReviewWeightCoefficient = 0.3;
        private const double TeamReviewWeightCoefficient = 0.2;
        private const double MentorSoftSkillReviewCoefficient = 0.02;
        private const double FinalCoefficient = 20;

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IProfileContext _context;

        public ProfileProvider(IProfileContext profileContext)
        {
            _context = profileContext;
            Logger.Debug("Creating instance of provider");
        }

        public TraineeProfile GetProfile(int profileId)
        {
            var profile = _context.TraineeProfiles.Find(profileId);
            return profile;
        }

        public TraineeProfile GetProfileByTraineeId(int traineeId)
        {
            var profile = _context.TraineeProfiles.FirstOrDefault(p => p.TraineeId == traineeId);
            return profile;
        }

        /// <summary>
        /// Attaches the given entity to the context underlying the set
        /// and updates changed fields in appropriate database entity
        /// that searches by the primary key value.
        /// </summary>
        /// <param name="profileToUpdate">Entity to update main information. Id must be declared.</param>
        /// <returns>Updated entity if exists or null.</returns>
        public TraineeProfile UpdateMainInfo(TraineeProfile profileToUpdate)
        {
            if (profileToUpdate == null) return null;

            var profile = _context.TraineeProfiles.Find(profileToUpdate.Id);

            // Expectations
            profile.DesirablePosition = profileToUpdate.DesirablePosition;
            profile.ExperienceAtITLab = profileToUpdate.ExperienceAtITLab;
            profile.ProfessionalPurposes = profileToUpdate.ProfessionalPurposes;

            // Experience
            profile.CurrentPosition = profileToUpdate.CurrentPosition;
            profile.CurrentWork = profileToUpdate.CurrentWork;
            profile.EmploymentDuration = profileToUpdate.EmploymentDuration;
            profile.JobDuties = profileToUpdate.JobDuties;

            // Contacts
            var newContacts = profileToUpdate.Trainee.User.Contacts;
            var updatedContacts = profile.Trainee.User.Contacts;

            updatedContacts.Email = newContacts.Email;
            updatedContacts.Phone = newContacts.Phone;
            updatedContacts.Skype = newContacts.Skype;
            updatedContacts.LinkedIn = newContacts.LinkedIn;

            _context.SaveChanges();

            return profile;
        }

        /// <summary>
        /// Attaches the given entity to the context underlying the set
        /// and updates changed fields in appropriate database entity
        /// that searches by the primary key value.
        /// </summary>
        /// <param name="profileToUpdate">Entity to update qualification information. Id must be declared.</param>
        /// <returns>Updated entity if exists or null.</returns>
        public TraineeProfile UpdateQualification(TraineeProfile profileToUpdate)
        {
            if (profileToUpdate != null)
            {
                _context.TraineeProfiles.Attach(profileToUpdate);

                var entry = _context.Entry(profileToUpdate);
                entry.Property(p => p.Strengths).IsModified = true;
                entry.Property(p => p.Weaknesses).IsModified = true;

                _context.SaveChanges();
            }

            return profileToUpdate;
        }

        public bool IsTraineeHavingMentorReview(int traineeId)
        {
            return IsTraineeHavingReview(traineeId, ReviewType.MentorReview);
        }

        public bool IsTraineeHavingScrumReview(int traineeId)
        {
            return IsTraineeHavingReview(traineeId, ReviewType.ScrumReview);
        }

        public bool IsTraineeHavingTeamReview(int traineeId)
        {
            return IsTraineeHavingReview(traineeId, ReviewType.TraineeReview);
        }

        public double GetTraineeRating(int traineeId)
        {
            // Calculates trainee rating according to the story: http://confluence.it-academy.by:8090/display/PROF/US+3.1.8+-+Rating
            var trainee = _context.Trainees.Find(traineeId);
            var groupmatesCount = trainee.Group.Trainees.Count(t => t.Id != traineeId);

            var traineeAllReviews = _context.Reviews
                .Where(t => t.ReviewedTraineeId == traineeId)
                .ToList();

            var traineeReviews = traineeAllReviews.Where(t => t.ReviewType == ReviewType.TraineeReview);
            var scrumReviews = traineeAllReviews.Where(t => t.ReviewType == ReviewType.ScrumReview);
            var mentorReviews = traineeAllReviews.Where(t => t.ReviewType == ReviewType.MentorReview);

            if (mentorReviews.Count() == 0 ||
                scrumReviews.Count() == 0 ||
                groupmatesCount != traineeReviews.Count())
            {
                return 0;
            }

            double traineeMark = CalculateReviewsRating(traineeReviews, TeamReviewWeightCoefficient, SkillType.Ability);
            double mentorMarkMainSkill = CalculateReviewsRating(mentorReviews, MentorReviewWeightCoefficient, SkillType.MainSkill);
            double mentorMarkSoftSkill = CalculateSumForMentorSoftSkillsReviews(mentorReviews) * MentorSoftSkillReviewCoefficient;
            double scrumMark = CalculateReviewsRating(scrumReviews, ScrumReviewWeightCoefficient, SkillType.Ability);

            return Math.Round((mentorMarkMainSkill + scrumMark + traineeMark + mentorMarkSoftSkill) * FinalCoefficient, 1);
        }

        private double CalculateReviewsRating(IEnumerable<Review> reviews, double coefficient, SkillType skillType)
        {
            return reviews
                   .Average(traineeReview => CalculateAvgReviewMark(traineeReview, skillType)) * coefficient;
        }

        private double CalculateAvgReviewMark(Review review, SkillType skillType)
        {
            var mark = review.Grades
                .Where(t => t.Mark != null && (t.Skill != null) ? t.Skill.SkillType == skillType : true)
                .Average(grade => grade.Mark);

            return mark.GetValueOrDefault();
        }

        private double CalculateSumForMentorSoftSkillsReviews(IEnumerable<Review> reviews)
        {
            return reviews
                   .Sum(r => r.Grades.FirstOrDefault(g => (g.Skill != null) ? g.Skill.SkillType == SkillType.SoftSkill : true).Mark.GetValueOrDefault());
        }

        private bool IsTraineeHavingReview(int traineeId, ReviewType reviewType)
        {
            return _context.Reviews.Any(r => r.ReviewedTraineeId == traineeId
                && r.ReviewType == reviewType);
        }
    }
}
