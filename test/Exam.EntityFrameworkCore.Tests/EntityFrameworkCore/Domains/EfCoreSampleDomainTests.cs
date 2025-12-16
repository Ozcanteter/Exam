using Exam.Samples;
using Xunit;

namespace Exam.EntityFrameworkCore.Domains;

[Collection(ExamTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ExamEntityFrameworkCoreTestModule>
{

}
