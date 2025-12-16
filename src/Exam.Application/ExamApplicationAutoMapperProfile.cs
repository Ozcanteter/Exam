using Exam.ChallengeUserTotals;
using Exam.ProgressEntries;
using Volo.Abp.Identity;
using Exam.Participants;
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

        CreateMap<Participant, ParticipantDto>();
        CreateMap<Participant, ParticipantExcelDto>();
        CreateMap<ParticipantWithNavigationProperties, ParticipantWithNavigationPropertiesDto>();
        CreateMap<Challenge, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));
        CreateMap<IdentityUser, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.UserName));

        CreateMap<ProgressEntry, ProgressEntryDto>();
        CreateMap<ProgressEntry, ProgressEntryExcelDto>();
        CreateMap<ProgressEntryWithNavigationProperties, ProgressEntryWithNavigationPropertiesDto>();

        CreateMap<ChallengeUserTotal, ChallengeUserTotalDto>();
        CreateMap<ChallengeUserTotal, ChallengeUserTotalExcelDto>();
        CreateMap<ChallengeUserTotalWithNavigationProperties, ChallengeUserTotalWithNavigationPropertiesDto>();
    }
}