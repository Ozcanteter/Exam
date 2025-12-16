using Exam.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.ChallengeUserTotals;

namespace Exam.Web.Pages.ChallengeUserTotals
{
    public class CreateModalModel : ExamPageModel
    {

        [BindProperty]
        public ChallengeUserTotalCreateViewModel ChallengeUserTotal { get; set; }

        public List<SelectListItem> ChallengeLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };
        public List<SelectListItem> IdentityUserLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IChallengeUserTotalsAppService _challengeUserTotalsAppService;

        public CreateModalModel(IChallengeUserTotalsAppService challengeUserTotalsAppService)
        {
            _challengeUserTotalsAppService = challengeUserTotalsAppService;

            ChallengeUserTotal = new();
        }

        public virtual async Task OnGetAsync()
        {
            ChallengeUserTotal = new ChallengeUserTotalCreateViewModel();
            ChallengeLookupListRequired.AddRange((
                                    await _challengeUserTotalsAppService.GetChallengeLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );
            IdentityUserLookupListRequired.AddRange((
                                    await _challengeUserTotalsAppService.GetIdentityUserLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _challengeUserTotalsAppService.CreateAsync(ObjectMapper.Map<ChallengeUserTotalCreateViewModel, ChallengeUserTotalCreateDto>(ChallengeUserTotal));
            return NoContent();
        }
    }

    public class ChallengeUserTotalCreateViewModel : ChallengeUserTotalCreateDto
    {
    }
}