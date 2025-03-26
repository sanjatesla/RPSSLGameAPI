using FluentAssertions;
using Moq;
using PSSLGame.Domain.Common;
using PSSLGame.Domain.Entities;
using PSSLGame.Domain.Repositories;
using PSSLGame.Domain.Services;
using RPSSLGame.Application.Commands;

namespace RPSSLGame.Test.UnitTests;

[TestFixture]
public class PlayGameHandlerTests
{
    private PlayGame.Handler _handler;
    private Mock<IChoiceGenerator> choiceGeneratorMock;
    private Mock<IScoreboardRepository> scoreboardRepoMock;

    [SetUp]
    public void Setup()
    {
        choiceGeneratorMock = new Mock<IChoiceGenerator>();
        choiceGeneratorMock.Setup(x => x.GenerateChoice())
            .Returns(Task.FromResult(Choices.Lizard));

        scoreboardRepoMock = new Mock<IScoreboardRepository>();
        scoreboardRepoMock.Setup(x => x.GetTopTenScores(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new List<ScoreEntity>()));

        _handler = new PlayGame.Handler(choiceGeneratorMock.Object, scoreboardRepoMock.Object);
    }

    [TestCase(Choices.Rock, Choices.Lizard, Results.Win)]
    [TestCase(Choices.Rock, Choices.Scissors, Results.Win)]
    [TestCase(Choices.Rock, Choices.Paper, Results.Lose)]
    [TestCase(Choices.Rock, Choices.Spock, Results.Lose)]
    [TestCase(Choices.Rock, Choices.Rock, Results.Tie)]
    [TestCase(Choices.Lizard, Choices.Paper, Results.Win)]
    [TestCase(Choices.Paper, Choices.Scissors, Results.Lose)]
    [TestCase(Choices.Spock, Choices.Scissors, Results.Win)]
    [TestCase(Choices.Spock, Choices.Lizard, Results.Lose)]
    public async Task PlayGame_ValidChoice_ShouldReturnOk(Choices playerChoice, Choices computerChoice, Results expectedResult)
    {
        // Arrange
        choiceGeneratorMock.Setup(x => x.GenerateChoice())
            .Returns(Task.FromResult(computerChoice));

        var command = new PlayGame.Command(playerChoice);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.Result.Should().BeEquivalentTo(expectedResult.ToString());
        if (expectedResult is Results.Win or Results.Lose)
            scoreboardRepoMock.Verify(x => x.AddScore(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task PlayGame_InValidChoice_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new PlayGame.Command(0);

        // Act & Assert
        var ex = Assert.ThrowsAsync<System.Collections.Generic.KeyNotFoundException>(async () =>
            await _handler.Handle(command, CancellationToken.None)
        );
        ex.Message.Should().Be("The given key '0' was not present in the dictionary.");
    }
}