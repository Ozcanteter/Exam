using System;

namespace Exam.ProgressEntries;

[Serializable]
public class ProgressEntryExcelDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}