using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Exam.Participants;
using Exam.Shared;

namespace Exam.Web.Pages.Participants
{
    public class IndexModel : AbpPageModel
    {
        [SelectItems(nameof(IsActiveBoolFilterItems))]
        public string IsActiveFilter { get; set; }

        public List<SelectListItem> IsActiveBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };
        [SelectItems(nameof(ChallengeLookupList))]
        public Guid ChallengeIdFilter { get; set; }
        public List<SelectListItem> ChallengeLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        [SelectItems(nameof(IdentityUserLookupList))]
        public Guid? IdentityUserIdFilter { get; set; }
        public List<SelectListItem> IdentityUserLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        protected IParticipantsAppService _participantsAppService;

        public IndexModel(IParticipantsAppService participantsAppService)
        {
            _participantsAppService = participantsAppService;
        }

        public virtual async Task OnGetAsync()
        {
            ChallengeLookupList.AddRange((
                    await _participantsAppService.GetChallengeLookupAsync(new LookupRequestDto
                    {
                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
            );

            IdentityUserLookupList.AddRange((
                            await _participantsAppService.GetIdentityUserLookupAsync(new LookupRequestDto
                            {
                                MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                            })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                    );

            await Task.CompletedTask;
        }
    }
}