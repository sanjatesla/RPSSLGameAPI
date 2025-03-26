using RPSSLGame.Application.Commands;
using RPSSLGame.Test.FunctionalTests.Base;
using FluentAssertions;
using System.Net;
using RPSSLGame.Application.Queries;

namespace RPSSLGame.Test.FunctionalTests;

[TestFixture]
public class PlayGameMultiplePlayersTests : TestBase
{
    [Test]
    public async Task PlayGameMultiplePlayers_WhenPlayersCountIsLessThanTwo_ShouldBeBadRequest()
    {
        // Arrange
        var players = new List<PlayGameMultiplePlayers.Player>()
        {
            new PlayGameMultiplePlayers.Player("player1", PSSLGame.Domain.Entities.Choice.Scissors)
        };

        var command = new PlayGameMultiplePlayers.MultiplePlayersCommand(players);

        // Act
        var response = await Client.PostAsync("/play-multiple", GetStringContent(command));
        var result = await response.Content.ReadAsStringAsync();
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeEmpty();
        result.Should().Contain("Min number of players is 2!");
    }

    [Test]
    public async Task PlayGameMultiplePlayers_PlayersListEmpty_ShouldBeBadRequest()
    {
        // Arrange
        var players = new List<PlayGameMultiplePlayers.Player>();

        var command = new PlayGameMultiplePlayers.MultiplePlayersCommand(players);

        // Act
        var response = await Client.PostAsync("/play-multiple", GetStringContent(command));
        var result = await response.Content.ReadAsStringAsync();
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeEmpty();
        result.Should().Contain("Players list must not be null or empty!");
    }

    [Test]
    public async Task PlayGameMultiplePlayers_PlayerNameEmpty_ShouldBeBadRequest()
    {
        // Arrange
        var players = new List<PlayGameMultiplePlayers.Player>()
        {
            new PlayGameMultiplePlayers.Player("player1", PSSLGame.Domain.Entities.Choice.Scissors),
            new PlayGameMultiplePlayers.Player("", PSSLGame.Domain.Entities.Choice.Lizard),
        };

        var command = new PlayGameMultiplePlayers.MultiplePlayersCommand(players);

        // Act
        var response = await Client.PostAsync("/play-multiple", GetStringContent(command));
        var result = await response.Content.ReadAsStringAsync();
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeEmpty();
        result.Should().Contain("Player Name is required!");
    }

    [Test]
    public async Task PlayGameMultiplePlayers_Scoreboard_AllValid_ShouldBeOk()
    {
        // Arrange
        var players = new List<PlayGameMultiplePlayers.Player>()
        {
            new PlayGameMultiplePlayers.Player("player1", PSSLGame.Domain.Entities.Choice.Rock),
            new PlayGameMultiplePlayers.Player("player2", PSSLGame.Domain.Entities.Choice.Scissors),
            new PlayGameMultiplePlayers.Player("player3", PSSLGame.Domain.Entities.Choice.Lizard),
        };

        var command = new PlayGameMultiplePlayers.MultiplePlayersCommand(players);

        // Act
        // play game
        var response = await Client.PostAsync("/play-multiple", GetStringContent(command));
        var result = await ConvertResponse<PlayGameMultiplePlayers.MultiplePlayersResponse>(response);
        // get scoreboard
        var scoreboardResponse = await Client.GetAsync("scoreboard");
        var resultScoreboard = await ConvertResponse<List<GetTopTenScoreboard.Response>>(scoreboardResponse);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Winners.Should().NotBeNullOrEmpty();
        result.Winners.FirstOrDefault().Name.Should().Be("player1");

        scoreboardResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        resultScoreboard.Should().NotBeNullOrEmpty();
        resultScoreboard.FirstOrDefault().Player.Should().Be("player1");
    }
}
