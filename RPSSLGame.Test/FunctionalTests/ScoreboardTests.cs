using RPSSLGame.Application.Commands;
using RPSSLGame.Test.FunctionalTests.Base;
using FluentAssertions;
using System.Net;
using RPSSLGame.Application.Queries;
using PSSLGame.Domain.Common;

namespace RPSSLGame.Test.FunctionalTests;

[TestFixture]
public class ScoreboardTests : TestBase
{
    [Test]
    public async Task Scoreboard_Top10_ShouldBeOk()
    {
        var players = new List<PlayGameMultiplePlayers.Player>()
            {
                new PlayGameMultiplePlayers.Player($"winner1", Choices.Rock),
                new PlayGameMultiplePlayers.Player("player2", Choices.Scissors),
                new PlayGameMultiplePlayers.Player("player3", Choices.Lizard),
            };
        var command = new PlayGameMultiplePlayers.MultiplePlayersCommand(players);
        // play game
        var response = await Client.PostAsync("/play-multiple", GetStringContent(command));

        for (int i = 1; i <= 15; i++)
        {
            players = new List<PlayGameMultiplePlayers.Player>()
            {
                new PlayGameMultiplePlayers.Player($"winner{i}", Choices.Rock),
                new PlayGameMultiplePlayers.Player("player2", Choices.Scissors),
                new PlayGameMultiplePlayers.Player("player3", Choices.Lizard),
            };
            command = new PlayGameMultiplePlayers.MultiplePlayersCommand(players);
            // play game
            response = await Client.PostAsync("/play-multiple", GetStringContent(command));
        }

        // get scoreboard
        var scoreboardResponse = await Client.GetAsync("scoreboard");
        var scoreboard = await ConvertResponse<List<GetTopTenScoreboard.Response>>(scoreboardResponse);

        scoreboardResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        scoreboard.Should().NotBeNullOrEmpty();
        scoreboard.Should().HaveCount(10);
        scoreboard.FirstOrDefault().Player.Should().Be("winner1");
        scoreboard.FirstOrDefault().Score.Should().Be(2);

        // reset scoreboard
        var resetResponse = await Client.PostAsync("reset-scoreboard", GetStringContent(new ResetScoreboard.ResetScoreboardCommand()));
        resetResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // get scoreboard
        scoreboardResponse = await Client.GetAsync("scoreboard");
        scoreboard = await ConvertResponse<List<GetTopTenScoreboard.Response>>(scoreboardResponse);
        scoreboardResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        scoreboard.Should().BeEmpty();
    }
}
