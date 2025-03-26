using PSSLGame.Domain.Entities;

namespace PSSLGame.Domain.Repositories;

public interface IScoreboardRepository
{
    Task<List<ScoreEntity>> GetTopTenScores(CancellationToken cancellationToken);
    Task ResetScoreboard(CancellationToken cancellationToken);
    Task AddScore(string player, CancellationToken cancellationToken);
}
