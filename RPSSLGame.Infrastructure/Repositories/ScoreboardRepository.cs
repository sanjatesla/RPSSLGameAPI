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

    public async Task AddScore(string player, CancellationToken cancellationToken)
    {
        await _scoreboardInMemory.AddScore(player);
    }

    public async Task<List<ScoreEntity>> GetTopTenScores(CancellationToken cancellationToken)
    {
        var scores = (await _scoreboardInMemory.GetScoreboard())
            .Select(x => new ScoreEntity(x.Key, x.Value))
            .OrderByDescending(x => x.Score)
            .Take(10);

        return scores.ToList();
    }

    public async Task ResetScoreboard(CancellationToken cancellationToken)
    {
        await _scoreboardInMemory.ResetScoreboard();
    }
}
