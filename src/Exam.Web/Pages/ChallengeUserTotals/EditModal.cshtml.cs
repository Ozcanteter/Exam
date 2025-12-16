using Exam.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Exam.ChallengeUserTotals;

namespace Exam.Web.Pages.ChallengeUserTotals
{
    public class EditModalModel : ExamPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ChallengeUserTotalUpdateViewModel ChallengeUserTotal { get; set; }

        public List<SelectListItem> ChallengeLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };
        public List<SelectListItem> IdentityUserLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IChallengeUserTotalsAppService _challengeUserTotalsAppService;

        public EditModalModel(IChallengeUserTotalsAppService challengeUserTotalsAppService)
        {
            _challengeUserTotalsAppService = challengeUserTotalsAppService;

            ChallengeUserTotal = new();
        }

        public virtual async Task OnGetAsync()
        {
            var challengeUserTotalWithNavigationPropertiesDto = await _challengeUserTotalsAppService.GetWithNavigationPropertiesAsync(Id);
            ChallengeUserTotal = ObjectMapper.Map<ChallengeUserTotalDto, ChallengeUserTotalUpdateViewModel>(challengeUserTotalWithNavigationPropertiesDto.ChallengeUserTotal);

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

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _challengeUserTotalsAppService.UpdateAsync(Id, ObjectMapper.Map<ChallengeUserTotalUpdateViewModel, ChallengeUserTotalUpdateDto>(ChallengeUserTotal));
            return NoContent();
        }
    }

    public class ChallengeUserTotalUpdateViewModel : ChallengeUserTotalUpdateDto
    {
    }
}