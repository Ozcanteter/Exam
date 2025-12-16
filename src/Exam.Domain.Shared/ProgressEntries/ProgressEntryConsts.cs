namespace Exam.ProgressEntries
{
    public static class ProgressEntryConsts
    {
        private const string DefaultSorting = "{0}Value asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "ProgressEntry." : string.Empty);
        }

    }
}