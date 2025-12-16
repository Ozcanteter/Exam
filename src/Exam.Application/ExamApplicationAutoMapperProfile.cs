using System;
using Exam.Shared;
using Volo.Abp.AutoMapper;
using Exam.Challenges;
using AutoMapper;

namespace Exam;

public class ExamApplicationAutoMapperProfile : Profile
{
    public ExamApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Challenge, ChallengeDto>();
        CreateMap<Challenge, ChallengeExcelDto>();
    }
}