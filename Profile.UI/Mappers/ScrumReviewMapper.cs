using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models;
using Profile.UI.Models.Json;
using Profile.UI.Models.Review;

namespace Profile.UI.Mappers
{
    public class ScrumReviewMapper
    {
        private IMapper _mapper;

        public ScrumReviewMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ScrumTraineesViewModel ReviewToEditViewModel(ScrumMaster scrumMaster, List<Review> reviews)
        {
            return new ScrumTraineesViewModel
            {
                ScrumMasterId = scrumMaster.Id,
                Reviews = reviews.Select(_mapper.Map<Review, ScrumReviewEditViewModel>).ToList()
            };
        }

        public Review JsonToBLModel(ScrumReviewJsonModel reviewJson)
        {
            var review = _mapper.Map<ScrumReviewJsonModel, Review>(reviewJson);

            review.Grades.Add(new Grade
            {
                Comment = reviewJson.Comment,
                Mark = reviewJson.Mark
            });

            return review;
        }

        public ScrumReviewViewModel ReviewToViewModel(Review review)
        {
            return _mapper.Map<Review, ScrumReviewViewModel>(review);
        }

        public LinkedScrumReviewViewModel ReviewToLinkedViewModel(Review review)
        {
            return _mapper.Map<Review, LinkedScrumReviewViewModel>(review);
        }
    }
}