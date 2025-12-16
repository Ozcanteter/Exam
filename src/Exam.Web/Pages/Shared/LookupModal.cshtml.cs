using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exam.Web.Pages.Shared
{
    public class LookupModal : ExamPageModel
    {
        public string CurrentId { get; set; }
        public string CurrentDisplayName { get; set; }

        public LookupModal()
        {
            CurrentId = string.Empty;
            CurrentDisplayName = string.Empty;
        }

        public virtual Task OnGetAsync(string currentId, string currentDisplayName)
        {
            CurrentId = currentId;
            CurrentDisplayName = currentDisplayName;

            return Task.CompletedTask;
        }
    }
}