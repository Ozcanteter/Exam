using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Exam.Challenges;
using Exam.Shared;

namespace Exam.Web.Pages.Challenges
{
    public class IndexModel : AbpPageModel
    {
        public string? NameFilter { get; set; }
        public DateTime? StartDateFilterMin { get; set; }

        public DateTime? StartDateFilterMax { get; set; }
        public DateTime? EndDateFilterMin { get; set; }

        public DateTime? EndDateFilterMax { get; set; }
        public double? GoalFilterMin { get; set; }

        public double? GoalFilterMax { get; set; }
        [SelectItems(nameof(IsActiveBoolFilterItems))]
        public string IsActiveFilter { get; set; }

        public List<SelectListItem> IsActiveBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };

        protected IChallengesAppService _challengesAppService;

        public IndexModel(IChallengesAppService challengesAppService)
        {
            _challengesAppService = challengesAppService;
        }

        public virtual async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}