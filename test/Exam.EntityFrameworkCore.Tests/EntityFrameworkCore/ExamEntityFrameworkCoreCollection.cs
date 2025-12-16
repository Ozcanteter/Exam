using Xunit;

namespace Exam.EntityFrameworkCore;

[CollectionDefinition(ExamTestConsts.CollectionDefinitionName)]
public class ExamEntityFrameworkCoreCollection : ICollectionFixture<ExamEntityFrameworkCoreFixture>
{

}
