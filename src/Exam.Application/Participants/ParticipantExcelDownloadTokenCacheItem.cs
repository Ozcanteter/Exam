using System;

namespace Exam.Participants;

[Serializable]
public class ParticipantExcelDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}