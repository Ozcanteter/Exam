using Exam.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Challenges;

namespace Exam.Web.Pages.Challenges
{
    public class CreateModalModel : ExamPageModel
    {

        [BindProperty]
        public ChallengeCreateViewModel Challenge { get; set; }

        protected IChallengesAppService _challengesAppService;

        public CreateModalModel(IChallengesAppService challengesAppService)
        {
            _challengesAppService = challengesAppService;

            Challenge = new();
        }

        public virtual async Task OnGetAsync()
        {
            Challenge = new ChallengeCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _challengesAppService.CreateAsync(ObjectMapper.Map<ChallengeCreateViewModel, ChallengeCreateDto>(Challenge));
            return NoContent();
        }
    }

    public class ChallengeCreateViewModel : ChallengeCreateDto
    {
    }
}