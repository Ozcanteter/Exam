using Volo.Abp.Application.Dtos;
using System;

namespace Exam.Challenges
{
    public class GetChallengesInput : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Name { get; set; }
        public DateTime? StartDateMin { get; set; }
        public DateTime? StartDateMax { get; set; }
        public DateTime? EndDateMin { get; set; }
        public DateTime? EndDateMax { get; set; }
        public double? GoalMin { get; set; }
        public double? GoalMax { get; set; }
        public bool? IsActive { get; set; }

        public GetChallengesInput()
        {

        }
    }
}