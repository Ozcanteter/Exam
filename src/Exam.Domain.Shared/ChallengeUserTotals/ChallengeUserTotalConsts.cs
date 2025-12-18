namespace Exam.ChallengeUserTotals
{
    public static class ChallengeUserTotalConsts
    {
        private const string DefaultSorting = "{0}TotalValue desc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "ChallengeUserTotal." : string.Empty);
        }

    }
}