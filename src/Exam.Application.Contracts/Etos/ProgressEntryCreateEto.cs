using System;
using Volo.Abp.EventBus;

namespace Exam.Etos
{
    [EventName("Exam.Etos.ProgressEntryCreateEto")]

    public class ProgressEntryCreateEto
    {
        public Guid UserId { get; set; }
        public double Value { get; set; }
        public Guid ChallengeId { get; set; }
    }
}
