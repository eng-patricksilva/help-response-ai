using HelpResponse.AI.Domain.Challenges.Requests;
using HelpResponse.AI.Domain.Challenges.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpResponse.AI.Services.Challenge
{
    public interface IChallengeService
    {
        public Task<IEnumerable<ChallengeResponse>> GetChallenges();
        public Task<ChallengeResponse> GetChallenge(int id);
        public Task<ChallengeResponse> CreateChallenge(ChallengeRequest request);
    }
}