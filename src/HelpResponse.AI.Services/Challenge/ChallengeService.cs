using AutoMapper;
using HelpResponse.AI.Domain.Challenges.Requests;
using HelpResponse.AI.Domain.Challenges.Responses;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChallengeDomain = HelpResponse.AI.Domain.Challenges.Challenge;

namespace HelpResponse.AI.Services.Challenge
{
    public class ChallengeService(ILogger<ChallengeService> looger, IMapper mapper) : IChallengeService
    {
        private readonly ILogger<ChallengeService> _logger = looger;
        private readonly IMapper _mapper = mapper;

        public async Task<ChallengeResponse> CreateChallenge(ChallengeRequest request)
        {
            await Task.Delay(10);

            var challenge = new ChallengeDomain(request.Id);
            _logger.LogInformation("Challenge created successfully. {@challenge}", challenge);

            var response = _mapper.Map<ChallengeResponse>(challenge);

            return response;
        }

        public async Task<ChallengeResponse> GetChallenge(int id)
        {
            await Task.Delay(10);

            var challenge = new ChallengeDomain(id);
            _logger.LogInformation("Challenge retrieved successfully. {@challenge}", challenge);

            var response = _mapper.Map<ChallengeResponse>(challenge);
            return response;
        }

        public async Task<IEnumerable<ChallengeResponse>> GetChallenges()
        {
            await Task.Delay(10);

            List<ChallengeDomain> challenges = [
                new(1),
                new(2),
            ];

            _logger.LogInformation("Challenge retrieved successfully. {@challenges}", challenges);

            var response = _mapper.Map<IEnumerable<ChallengeResponse>>(challenges);
            return response;
        }
    }
}