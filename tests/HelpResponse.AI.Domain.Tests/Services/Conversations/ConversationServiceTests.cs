using FluentAssertions;
using HelpResponse.AI.Domain.Conversations.Requests;
using HelpResponse.AI.Services.Conversations;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;

namespace HelpResponse.AI.Tests.Services.Conversations;

public class ConversationServiceTests
{
    private readonly Mock<ILogger<ConversationService>> loggerMock;

    public ConversationServiceTests()
    {
    }

    [Fact]
    public async Task ProccessConversationsAsync_WhenProccessConversations_ShouldReturnSuccess()
    {
        //Arrange
        var conversationServices = new ConversationService(loggerMock.Object);
        var request = new ConversationRequest
        {

        };

        //Act
        var result = await conversationServices.CreateConversation(new());

        // Assert
        result.Should().NotBeNull();
        result.Messages.Should().NotBeEmpty();
    }
}