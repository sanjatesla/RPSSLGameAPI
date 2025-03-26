namespace RPSSLGame.Infrastructure;

public class ScoreboardInMemory
{
    private readonly Dictionary<string, int> Scoreboard = new();
    public async Task AddScore(string playerName)
    {
        if (Scoreboard.ContainsKey(playerName))
            Scoreboard[playerName]++;
        else
            Scoreboard[playerName] = 1;
        await Task.CompletedTask;
    }

    public async Task<Dictionary<string, int>> GetScoreboard() => await Task.FromResult(Scoreboard);

    public async Task ResetScoreboard()
    {
        Scoreboard.Clear();
        await Task.CompletedTask;
    }
}
