using Exam.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Exam.Challenges;

namespace Exam.Web.Pages.Challenges
{
    public class EditModalModel : ExamPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ChallengeUpdateViewModel Challenge { get; set; }

        protected IChallengesAppService _challengesAppService;

        public EditModalModel(IChallengesAppService challengesAppService)
        {
            _challengesAppService = challengesAppService;

            Challenge = new();
        }

        public virtual async Task OnGetAsync()
        {
            var challenge = await _challengesAppService.GetAsync(Id);
            Challenge = ObjectMapper.Map<ChallengeDto, ChallengeUpdateViewModel>(challenge);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _challengesAppService.UpdateAsync(Id, ObjectMapper.Map<ChallengeUpdateViewModel, ChallengeUpdateDto>(Challenge));
            return NoContent();
        }
    }

    public class ChallengeUpdateViewModel : ChallengeUpdateDto
    {
    }
}