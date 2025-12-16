namespace Exam.Challenges
{
    public static class ChallengeConsts
    {
        private const string DefaultSorting = "{0}Name asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Challenge." : string.Empty);
        }

        public const int NameMaxLength = 2048;
    }
}