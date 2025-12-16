using Exam.Samples;
using Xunit;

namespace Exam.EntityFrameworkCore.Applications;

[Collection(ExamTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ExamEntityFrameworkCoreTestModule>
{

}
