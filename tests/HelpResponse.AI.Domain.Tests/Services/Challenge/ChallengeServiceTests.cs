using AutoMapper;
using FluentAssertions;
using HelpResponse.AI.Domain.Challenges.Mappers;
using HelpResponse.AI.Domain.Challenges.Requests;
using HelpResponse.AI.Services.Challenge;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;

namespace HelpResponse.AI.Tests.Services.Balance;

public class ChallengeServiceTests
{
    private readonly IMapper mapper;
    private readonly Mock<ILogger<ChallengeService>> loggerMock;

    public ChallengeServiceTests()
    {
        loggerMock = new Mock<ILogger<ChallengeService>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ChallengeMapperProfile());
        });

        mapper = config.CreateMapper();
    }

    [Fact]
    public async Task ProccessBalanceAsync_WhenProccessBalance_ShouldReturnProcessed()
    {
        //Arrange
        var challengeServices = new ChallengeService(loggerMock.Object, mapper);
        var request = new ChallengeRequest
        {
            Id = 1,
        };

        //Act
        var result = await challengeServices.GetChallenge(request.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(request.Id);
    }
}