using System.Text;

namespace Profile.UI.HtmlHelpers
{
    public class ReviewTabsBuilder
    {
        private readonly StringBuilder _stringBuilder;
        private bool _isActiveTabBuilt;

        public ReviewTabsBuilder()
        {
            _stringBuilder = new StringBuilder();
        }

        public string Build(
            bool isTraineeHavingMentorReview,
            bool isTraineeHavingScrumReview,
            bool isTraineeHavingTeamReview)
        {
            _isActiveTabBuilt = false;
            _stringBuilder.Clear();

            _stringBuilder.AppendLine(@"<ul class='row roleTabs'>");

            BuildTab(@"#teamreview&skill1", "Отзывы от Команды", isTraineeHavingTeamReview);
            BuildTab(@"#mentorreview", "Отзыв от Ментора", isTraineeHavingMentorReview);
            BuildTab(@"#scrumreview", "Отзыв от Скрам-Мастера", isTraineeHavingScrumReview);

            _stringBuilder.AppendLine(@"</ul>");

            return _stringBuilder.ToString();
        }

        private void BuildTab(
            string hrefValue,
            string innerTextValue,
            bool isTraineeHavingReview)
        {
            string tabClass = string.Empty;
            string hrefAttr = isTraineeHavingReview ? $"href='{hrefValue}'" : string.Empty;

            if (!isTraineeHavingReview)
            {
                tabClass = "disabled";
            }
            else if (!_isActiveTabBuilt)
            {
                tabClass = "active";
                _isActiveTabBuilt = true;
            }

            _stringBuilder.AppendLine($@"<li class='col-xs-4 {tabClass}'>");
            _stringBuilder.AppendLine($@"<a {hrefAttr}><span>{innerTextValue}</span></a>");
            _stringBuilder.AppendLine(@"</li>");
        }
}
}