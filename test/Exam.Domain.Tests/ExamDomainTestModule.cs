using Volo.Abp.Modularity;

namespace Exam;

[DependsOn(
    typeof(ExamDomainModule),
    typeof(ExamTestBaseModule)
)]
public class ExamDomainTestModule : AbpModule
{

}
