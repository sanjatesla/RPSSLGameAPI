using RPSSLGame.Application.Commands;
using RPSSLGame.Test.InterationTests.Base;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace RPSSLGame.Test.InterationTests;

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
        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("/play-multiple", content);
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
        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("/play-multiple", content);
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
        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("/play-multiple", content);
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
        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        // play game
        var response = await Client.PostAsync("/play-multiple", content);
        var result = await response.Content.ReadAsStringAsync();
        // get scoreboard
        var scoreboardResponse = await Client.GetAsync("scoreboard");
        var resultScoreboard = await scoreboardResponse.Content.ReadAsStringAsync();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("player1");

        scoreboardResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        resultScoreboard.Should().NotBeNullOrEmpty();
        resultScoreboard.Should().Contain("player1");
    }
}
