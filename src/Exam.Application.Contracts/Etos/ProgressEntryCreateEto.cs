using System;
using System.Text.Json.Serialization;
using Volo.Abp.EventBus;

namespace Exam.Etos
{
    [EventName("Exam.Etos.ProgressEntryCreateEto")]

    public class ProgressEntryCreateEto
    {
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }

        [JsonPropertyName("challengeId")]
        public Guid ChallengeId { get; set; }
    }
}
