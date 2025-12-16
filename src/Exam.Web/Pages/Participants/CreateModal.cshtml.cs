using Exam.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Participants;

namespace Exam.Web.Pages.Participants
{
    public class CreateModalModel : ExamPageModel
    {

        [BindProperty]
        public ParticipantCreateViewModel Participant { get; set; }

        public List<SelectListItem> ChallengeLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };
        public List<SelectListItem> IdentityUserLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(" — ", "")
        };

        protected IParticipantsAppService _participantsAppService;

        public CreateModalModel(IParticipantsAppService participantsAppService)
        {
            _participantsAppService = participantsAppService;

            Participant = new();
        }

        public virtual async Task OnGetAsync()
        {
            Participant = new ParticipantCreateViewModel();
            ChallengeLookupListRequired.AddRange((
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

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _participantsAppService.CreateAsync(ObjectMapper.Map<ParticipantCreateViewModel, ParticipantCreateDto>(Participant));
            return NoContent();
        }
    }

    public class ParticipantCreateViewModel : ParticipantCreateDto
    {
    }
}