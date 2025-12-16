using Exam.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Exam.ProgressEntries;

namespace Exam.Web.Pages.ProgressEntries
{
    public class EditModalModel : ExamPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ProgressEntryUpdateViewModel ProgressEntry { get; set; }

        public List<SelectListItem> ChallengeLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };
        public List<SelectListItem> IdentityUserLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IProgressEntriesAppService _progressEntriesAppService;

        public EditModalModel(IProgressEntriesAppService progressEntriesAppService)
        {
            _progressEntriesAppService = progressEntriesAppService;

            ProgressEntry = new();
        }

        public virtual async Task OnGetAsync()
        {
            var progressEntryWithNavigationPropertiesDto = await _progressEntriesAppService.GetWithNavigationPropertiesAsync(Id);
            ProgressEntry = ObjectMapper.Map<ProgressEntryDto, ProgressEntryUpdateViewModel>(progressEntryWithNavigationPropertiesDto.ProgressEntry);

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

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _progressEntriesAppService.UpdateAsync(Id, ObjectMapper.Map<ProgressEntryUpdateViewModel, ProgressEntryUpdateDto>(ProgressEntry));
            return NoContent();
        }
    }

    public class ProgressEntryUpdateViewModel : ProgressEntryUpdateDto
    {
    }
}