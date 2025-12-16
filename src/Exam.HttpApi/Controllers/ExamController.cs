using Exam.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Exam.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ExamController : AbpControllerBase
{
    protected ExamController()
    {
        LocalizationResource = typeof(ExamResource);
    }
}
