namespace RPSSLGame.Infrastructure;

public class ScoreboardInMemory
{
    private readonly Dictionary<string, int> Scoreboard = new();

    /// <summary>Adds the score.</summary>
    /// <param name="playerName">Name of the player.</param>
    public async Task AddScore(string playerName)
    {
        if (Scoreboard.ContainsKey(playerName))
            Scoreboard[playerName]++;
        else
            Scoreboard[playerName] = 1;
        await Task.CompletedTask;
    }

    public async Task<Dictionary<string, int>> GetScoreboard() => await Task.FromResult(Scoreboard);

    /// <summary>Resets the scoreboard.</summary>
    public async Task ResetScoreboard()
    {
        Scoreboard.Clear();
        await Task.CompletedTask;
    }
}
