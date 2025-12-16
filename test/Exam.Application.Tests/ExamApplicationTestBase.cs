using Volo.Abp.Modularity;

namespace Exam;

public abstract class ExamApplicationTestBase<TStartupModule> : ExamTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
