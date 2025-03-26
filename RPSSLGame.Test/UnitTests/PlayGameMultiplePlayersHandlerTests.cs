using FluentAssertions;
using Moq;
using PSSLGame.Domain.Common;
using PSSLGame.Domain.Entities;
using PSSLGame.Domain.Repositories;
using PSSLGame.Domain.Services;
using RPSSLGame.Application.Commands;

namespace RPSSLGame.Test.UnitTests;

[TestFixture]
public class PlayGameMultiplePlayersHandlerTests
{
    private PlayGameMultiplePlayers.Handler _handler;
    private Mock<IScoreboardRepository> scoreboardRepoMock;

    [SetUp]
    public void Setup()
    {
        scoreboardRepoMock = new Mock<IScoreboardRepository>();
        scoreboardRepoMock.Setup(x => x.GetTopTenScores(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new List<ScoreEntity>()));

        _handler = new PlayGameMultiplePlayers.Handler(scoreboardRepoMock.Object);
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
    public async Task PlayGameMultiplePlayers_2Players_Valid_ShouldReturnOk(Choices player1Choice, Choices player2Choice, Results expectedResult)
    {
        // Arrange
        var players = new List<PlayGameMultiplePlayers.Player>()
        {
            new PlayGameMultiplePlayers.Player("player1", player1Choice),
            new PlayGameMultiplePlayers.Player("player2", player2Choice),
        };

        var command = new PlayGameMultiplePlayers.MultiplePlayersCommand(players);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        if (expectedResult is Results.Win)
        {
            response.Winners.Should().NotBeEmpty();
            response.Winners[0].Name.Should().Be("player1");
            scoreboardRepoMock.Verify(x => x.AddScore("player1", It.IsAny<CancellationToken>()), Times.Once());
        }
        if (expectedResult is Results.Lose)
        {
            response.Winners.Should().NotBeEmpty();
            response.Winners[0].Name.Should().Be("player2");
            scoreboardRepoMock.Verify(x => x.AddScore("player2", It.IsAny<CancellationToken>()), Times.Once());
        }
        if (expectedResult is Results.Tie)
            response.Winners.Should().BeEmpty();
    }

    [TestCase(Choices.Rock, Choices.Lizard, Choices.Scissors, 1)]
    [TestCase(Choices.Rock, Choices.Paper, Choices.Spock, 2)]
    [TestCase(Choices.Scissors, Choices.Rock, Choices.Spock, 3)]
    [TestCase(Choices.Scissors, Choices.Rock, Choices.Paper, 0)]
    public async Task PlayGameMultiplePlayers_3Players_Valid_ShouldReturnOk(Choices player1Choice,
        Choices player2Choice, Choices player3Choice, int winnerPlayer)
    {
        // Arrange
        var players = new List<PlayGameMultiplePlayers.Player>()
        {
            new PlayGameMultiplePlayers.Player("player1", player1Choice),
            new PlayGameMultiplePlayers.Player("player2", player2Choice),
            new PlayGameMultiplePlayers.Player("player3", player3Choice),
        };

        var command = new PlayGameMultiplePlayers.MultiplePlayersCommand(players);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        if (winnerPlayer != 0)
        {
            response.Winners.Should().NotBeEmpty();
            response.Winners[0].Name.Should().Be($"player{winnerPlayer}");
            scoreboardRepoMock.Verify(x => x.AddScore($"player{winnerPlayer}", It.IsAny<CancellationToken>()), Times.Once());
        }
        else
            response.Winners.Should().BeEmpty();
    }
}