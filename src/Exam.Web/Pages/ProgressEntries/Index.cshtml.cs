using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Exam.ProgressEntries;
using Exam.Shared;

namespace Exam.Web.Pages.ProgressEntries
{
    public class IndexModel : AbpPageModel
    {
        public double? ValueFilterMin { get; set; }

        public double? ValueFilterMax { get; set; }
        [SelectItems(nameof(ChallengeLookupList))]
        public Guid ChallengeIdFilter { get; set; }
        public List<SelectListItem> ChallengeLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        [SelectItems(nameof(IdentityUserLookupList))]
        public Guid IdentityUserIdFilter { get; set; }
        public List<SelectListItem> IdentityUserLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        protected IProgressEntriesAppService _progressEntriesAppService;

        public IndexModel(IProgressEntriesAppService progressEntriesAppService)
        {
            _progressEntriesAppService = progressEntriesAppService;
        }

        public virtual async Task OnGetAsync()
        {
            ChallengeLookupList.AddRange((
                    await _progressEntriesAppService.GetChallengeLookupAsync(new LookupRequestDto
                    {
                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
            );

            IdentityUserLookupList.AddRange((
                            await _progressEntriesAppService.GetIdentityUserLookupAsync(new LookupRequestDto
                            {
                                MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                            })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                    );

            await Task.CompletedTask;
        }
    }
}