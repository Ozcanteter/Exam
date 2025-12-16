using Volo.Abp.Modularity;

namespace Exam;

[DependsOn(
    typeof(ExamApplicationModule),
    typeof(ExamDomainTestModule)
)]
public class ExamApplicationTestModule : AbpModule
{

}
