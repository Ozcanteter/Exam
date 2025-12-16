using Exam.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Exam.Participants;

namespace Exam.Web.Pages.Participants
{
    public class EditModalModel : ExamPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ParticipantUpdateViewModel Participant { get; set; }

        public List<SelectListItem> ChallengeLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };
        public List<SelectListItem> IdentityUserLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(" — ", "")
        };

        protected IParticipantsAppService _participantsAppService;

        public EditModalModel(IParticipantsAppService participantsAppService)
        {
            _participantsAppService = participantsAppService;

            Participant = new();
        }

        public virtual async Task OnGetAsync()
        {
            var participantWithNavigationPropertiesDto = await _participantsAppService.GetWithNavigationPropertiesAsync(Id);
            Participant = ObjectMapper.Map<ParticipantDto, ParticipantUpdateViewModel>(participantWithNavigationPropertiesDto.Participant);

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

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _participantsAppService.UpdateAsync(Id, ObjectMapper.Map<ParticipantUpdateViewModel, ParticipantUpdateDto>(Participant));
            return NoContent();
        }
    }

    public class ParticipantUpdateViewModel : ParticipantUpdateDto
    {
    }
}