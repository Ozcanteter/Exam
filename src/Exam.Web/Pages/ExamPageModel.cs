using Exam.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Exam.Web.Pages;

public abstract class ExamPageModel : AbpPageModel
{
    protected ExamPageModel()
    {
        LocalizationResourceType = typeof(ExamResource);
    }
}
