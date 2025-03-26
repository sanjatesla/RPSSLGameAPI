using FluentAssertions;
using Moq;
using PSSLGame.Domain.Entities;
using PSSLGame.Domain.Repositories;
using PSSLGame.Domain.Services;
using RPSSLGame.Application.Commands;

namespace RPSSLGame.Test.UnitTests;

[TestFixture]
public class PlayGameMultiplePlayersHandlerTests
{
    private PlayGameMultiplePlayers.Handler _handler;
    private Mock<IChoiceGenerator> choiceGeneratorMock;
    private Mock<IScoreboardRepository> scoreboardRepoMock;

    [SetUp]
    public void Setup()
    {
        choiceGeneratorMock = new Mock<IChoiceGenerator>();
        choiceGeneratorMock.Setup(x => x.GenerateChoice())
            .Returns(Task.FromResult(Choice.Lizard));

        scoreboardRepoMock = new Mock<IScoreboardRepository>();
        scoreboardRepoMock.Setup(x => x.GetTopTenScores(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new List<ScoreEntity>()));

        _handler = new PlayGameMultiplePlayers.Handler(scoreboardRepoMock.Object);
    }

    [TestCase(Choice.Rock, Choice.Lizard, Results.Win)]
    [TestCase(Choice.Rock, Choice.Scissors, Results.Win)]
    [TestCase(Choice.Rock, Choice.Paper, Results.Lose)]
    [TestCase(Choice.Rock, Choice.Spock, Results.Lose)]
    [TestCase(Choice.Rock, Choice.Rock, Results.Tie)]
    [TestCase(Choice.Lizard, Choice.Paper, Results.Win)]
    [TestCase(Choice.Paper, Choice.Scissors, Results.Lose)]
    [TestCase(Choice.Spock, Choice.Scissors, Results.Win)]
    [TestCase(Choice.Spock, Choice.Lizard, Results.Lose)]
    public async Task PlayGameMultiplePlayers_2Players_Valid_ShouldReturnOk(Choice player1Choice, Choice player2Choice, Results expectedResult)
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

    [TestCase(Choice.Rock, Choice.Lizard, Choice.Scissors, 1)]
    [TestCase(Choice.Rock, Choice.Paper, Choice.Spock, 2)]
    [TestCase(Choice.Scissors, Choice.Rock, Choice.Spock, 3)]
    [TestCase(Choice.Scissors, Choice.Rock, Choice.Paper, 0)]
    public async Task PlayGameMultiplePlayers_3Players_Valid_ShouldReturnOk(Choice player1Choice,
        Choice player2Choice, Choice player3Choice, int winnerPlayer)
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