namespace Exam.Participants
{
    public static class ParticipantConsts
    {
        private const string DefaultSorting = "{0}IsActive asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Participant." : string.Empty);
        }

    }
}