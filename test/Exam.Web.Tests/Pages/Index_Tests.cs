using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Exam.Pages;

[Collection(ExamTestConsts.CollectionDefinitionName)]
public class Index_Tests : ExamWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
