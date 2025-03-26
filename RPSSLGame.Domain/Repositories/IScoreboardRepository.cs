using PSSLGame.Domain.Entities;

namespace PSSLGame.Domain.Repositories;

public interface IScoreboardRepository
{
    /// <summary>Gets the top ten scores.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<List<ScoreEntity>> GetTopTenScores(CancellationToken cancellationToken);
    /// <summary>Resets the scoreboard.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task ResetScoreboard(CancellationToken cancellationToken);
    /// <summary>Adds the score.</summary>
    /// <param name="player">The player.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AddScore(string player, CancellationToken cancellationToken);
}
