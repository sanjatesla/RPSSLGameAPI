using MediatR;
using PSSLGame.Domain.Repositories;

namespace RPSSLGame.Application.Queries;

public static class GetTopTenScoreboard
{
    #region Query

    public record Query : IRequest<List<Response>>;

    #endregion

    #region Handler

    public class Handler : IRequestHandler<Query, List<Response>>
    {
        private readonly IScoreboardRepository _scoreboardRepository;
        public Handler(IScoreboardRepository scoreboardRepository)
        {
            _scoreboardRepository = scoreboardRepository;
        }

        public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var scoreboard = await _scoreboardRepository.GetTopTenScores(cancellationToken);
            return scoreboard.Select(x => new Response(x.Player, x.Score)).ToList();
        }
    }

    #endregion

    #region Response

    public record Response(string Player, int Score);

    #endregion
}
