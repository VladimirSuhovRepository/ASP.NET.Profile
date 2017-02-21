using System.Linq;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models;
using Profile.UI.Models.Review;

namespace Profile.UI.Mappers
{
    public class MentorReviewMapper
    {
        private IMapper _mapper;

        public MentorReviewMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public MentorReviewViewModel ToViewModel(Review review)
        {
            var reviewViewModel = _mapper.Map<Review, MentorReviewViewModel>(review);

            foreach (var g in review.Grades.Where(g => g.SkillId.HasValue))
            {
                var grade = _mapper.Map<Grade, GradeViewModel>(g);

                if (g.Skill.SkillType == SkillType.SoftSkill)
                {
                    reviewViewModel.SoftSkillGrades.Add(grade);
                }
                else
                {
                    reviewViewModel.MainSkillGrades.Add(grade);
                }
            }

            return reviewViewModel;
        }

        public LinkedMentorReviewViewModel ToLinkedMentorReviewViewModel(Review review)
        {
            var reviewViewModel = new LinkedMentorReviewViewModel
            {
                Id = review.Id,
                MentorId = review.ReviewerId,
                MentorFullname = review.Reviewer.FullName,
                MentorSpecializationName = review.ReviewedTrainee.Specialization.Name,

                WorkComment = review.Grades
                    .FirstOrDefault(g => !g.SkillId.HasValue)?.Comment ?? string.Empty
            };

            foreach (var g in review.Grades.Where(g => g.SkillId.HasValue))
            {
                var grade = _mapper.Map<Grade, GradeViewModel>(g);

                if (g.Skill.SkillType == SkillType.SoftSkill)
                {
                    reviewViewModel.SoftSkillGrades.Add(grade);
                }
                else
                {
                    reviewViewModel.MainSkillGrades.Add(grade);
                }
            }

            return reviewViewModel;
        }

        public Review JsonToBLModel(MentorReviewJsonModel reviewJsonModel)
        {
            var review = _mapper.Map<MentorReviewJsonModel, Review>(reviewJsonModel);

            review.Grades = reviewJsonModel.SkillGrades
                .Select(g => _mapper.Map<GradeViewModel, Grade>(g))
                .ToList();

            review.Grades.Add(new Grade
            {
                Review = review,
                Comment = reviewJsonModel.WorkComment
            });

            return review;
        }
    }
}