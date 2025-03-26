using PSSLGame.Domain.Entities;
using PSSLGame.Domain.Repositories;

namespace RPSSLGame.Infrastructure.Repositories;

public class ScoreboardRepository : IScoreboardRepository
{
    private readonly ScoreboardInMemory _scoreboardInMemory;

    public ScoreboardRepository(ScoreboardInMemory scoreboardInMemory)
    {
        _scoreboardInMemory = scoreboardInMemory;
    }

    /// <summary>Adds the score.</summary>
    /// <param name="player">The player.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task AddScore(string player, CancellationToken cancellationToken)
    {
        await _scoreboardInMemory.AddScore(player);
    }

    /// <summary>Gets the top ten scores.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task<List<ScoreEntity>> GetTopTenScores(CancellationToken cancellationToken)
    {
        var scores = (await _scoreboardInMemory.GetScoreboard())
            .Select(x => new ScoreEntity(x.Key, x.Value))
            .OrderByDescending(x => x.Score)
            .Take(10);

        return scores.ToList();
    }

    /// <summary>Resets the scoreboard.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task ResetScoreboard(CancellationToken cancellationToken)
    {
        await _scoreboardInMemory.ResetScoreboard();
    }
}
