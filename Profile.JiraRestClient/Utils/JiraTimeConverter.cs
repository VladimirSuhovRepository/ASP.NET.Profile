namespace Profile.JiraRestClient.Utils
{
    internal static class JiraTimeConverter
    {
        public static string ToJiraTimeFormat(int seconds)
        {
            int weeks, days, hours, minutes = 0;
            string jiraTime = string.Empty;

            weeks = seconds / (int)SecondsIn.Week;
            if (weeks > 0)
            {
                seconds -= weeks * (int)SecondsIn.Week;
                jiraTime += $"{weeks}w ";
            }

            days = seconds / (int)SecondsIn.Day;
            if (days > 0)
            {
                seconds -= days * (int)SecondsIn.Day;
                jiraTime += $"{days}d ";
            }

            hours = seconds / (int)SecondsIn.Hour;
            if (hours > 0)
            {
                seconds -= hours * (int)SecondsIn.Hour;
                jiraTime += $"{hours}h ";
            }

            minutes = seconds / (int)SecondsIn.Minute;
            if (minutes > 0)
            {
                seconds -= minutes * (int)SecondsIn.Minute;
                jiraTime += $"{minutes}m";
            }

            return string.IsNullOrEmpty(jiraTime) ? null : jiraTime;
        }
    }
}
