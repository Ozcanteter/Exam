using Exam.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.ProgressEntries;

namespace Exam.Web.Pages.ProgressEntries
{
    public class CreateModalModel : ExamPageModel
    {

        [BindProperty]
        public ProgressEntryCreateViewModel ProgressEntry { get; set; }

        public List<SelectListItem> ChallengeLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };
        public List<SelectListItem> IdentityUserLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IProgressEntriesAppService _progressEntriesAppService;

        public CreateModalModel(IProgressEntriesAppService progressEntriesAppService)
        {
            _progressEntriesAppService = progressEntriesAppService;

            ProgressEntry = new();
        }

        public virtual async Task OnGetAsync()
        {
            ProgressEntry = new ProgressEntryCreateViewModel();
            ChallengeLookupListRequired.AddRange((
                                    await _progressEntriesAppService.GetChallengeLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );
            IdentityUserLookupListRequired.AddRange((
                                    await _progressEntriesAppService.GetIdentityUserLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _progressEntriesAppService.CreateAsync(ObjectMapper.Map<ProgressEntryCreateViewModel, ProgressEntryCreateDto>(ProgressEntry));
            return NoContent();
        }
    }

    public class ProgressEntryCreateViewModel : ProgressEntryCreateDto
    {
    }
}