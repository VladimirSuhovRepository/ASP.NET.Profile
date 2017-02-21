using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Profile.UI.HtmlHelpers
{
    public static class ReviewPageHelper
    {
        public static HtmlString BuildReviewTabs(
            this HtmlHelper helper, 
            bool isTraineeHavingMentorReview,
            bool isTraineeHavingScrumReview,
            bool isTraineeHavingTeamReview)
        {
            var tabBuilder = new ReviewTabsBuilder();

            string builtHtml = tabBuilder.Build(
                isTraineeHavingMentorReview, 
                isTraineeHavingScrumReview, 
                isTraineeHavingTeamReview);

            return new HtmlString(builtHtml);
        }
    }
}