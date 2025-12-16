using Exam.Localization;
using Volo.Abp.Application.Services;

namespace Exam;

/* Inherit your application services from this class.
 */
public abstract class ExamAppService : ApplicationService
{
    protected ExamAppService()
    {
        LocalizationResource = typeof(ExamResource);
    }
}
