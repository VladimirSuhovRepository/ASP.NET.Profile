using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models;
using Profile.UI.Models.Json;
using Profile.UI.Models.Review;

namespace Profile.UI.Mappers
{
    public class TraineeReviewMapper
    {
        private IMapper _mapper;

        public TraineeReviewMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public LinkedTeamReviewViewModel ToLinkedTeamReviewViewModel(
            List<Skill> abilities,
            List<Grade> grades)
        {
            var viewModel = new LinkedTeamReviewViewModel();

            foreach (var ability in abilities.OrderBy(a => a.SkillType))
            {
                var abilityViewModel = _mapper.Map<Skill, TeamReviewedSkillViewModel>(ability);

                abilityViewModel.SkillReviews = grades
                    .Where(g => g.SkillId.Value == ability.Id)
                    .Select(g => _mapper.Map<Grade, TeammateSkillGradeViewModel>(g))
                    .ToList();

                viewModel.Skills.Add(abilityViewModel);
            }

            return viewModel;
        }

        public TraineeReviewViewModel ToViewModel(Review review, List<Skill> abilities)
        {
            var traineeReview = _mapper.Map<Review, TraineeReviewViewModel>(review);

            traineeReview.Skills = _mapper.Map<List<Skill>, List<SkillViewModel>>(
                abilities.Where(a => a.SkillType == SkillType.Ability).ToList());

            traineeReview.Strengths = _mapper.Map<Skill, SkillViewModel>(
                abilities.Where(a => a.SkillType == SkillType.Strenghts)
                .SingleOrDefault());

            traineeReview.Weakness = _mapper.Map<Skill, SkillViewModel>(
                abilities.Where(a => a.SkillType == SkillType.Weaknesses)
                .SingleOrDefault());

            return traineeReview;
        }

        public Review FromJsonModel(TraineeReviewJsonModel reviewJsonModel)
        {
            return _mapper.Map<TraineeReviewJsonModel, Review>(reviewJsonModel);
        }
    }
}