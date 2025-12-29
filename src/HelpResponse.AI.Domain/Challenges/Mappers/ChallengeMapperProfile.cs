using AutoMapper;
using HelpResponse.AI.Domain.Challenges.Responses;

namespace HelpResponse.AI.Domain.Challenges.Mappers
{
    public class ChallengeMapperProfile : Profile
    {
        public ChallengeMapperProfile()
        {
            CreateMap<Challenge, ChallengeResponse>();
        }
    }
}