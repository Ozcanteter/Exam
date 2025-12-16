using Exam.Web.Pages.Challenges;
using Volo.Abp.AutoMapper;
using Exam.Challenges;
using AutoMapper;

namespace Exam.Web;

public class ExamWebAutoMapperProfile : Profile
{
    public ExamWebAutoMapperProfile()
    {
        //Define your object mappings here, for the Web project

        CreateMap<ChallengeDto, ChallengeUpdateViewModel>();
        CreateMap<ChallengeUpdateViewModel, ChallengeUpdateDto>();
        CreateMap<ChallengeCreateViewModel, ChallengeCreateDto>();
    }
}