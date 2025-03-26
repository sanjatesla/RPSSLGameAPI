using MediatR;
using PSSLGame.Domain.Repositories;

namespace RPSSLGame.Application.Commands;

public static class ResetScoreboard
{
    #region Command

    public record ResetScoreboardCommand() : IRequest<ResetScoreboardResponse>;

    #endregion

    #region Handler
    public class Handler : IRequestHandler<ResetScoreboardCommand, ResetScoreboardResponse>
    {
        private readonly IScoreboardRepository _scoreboardRepository;
        public Handler(IScoreboardRepository scoreboardRepository)
        {
            _scoreboardRepository = scoreboardRepository;
        }

        public async Task<ResetScoreboardResponse> Handle(ResetScoreboardCommand request, CancellationToken cancellationToken)
        {
            // reset scoreboard
            await _scoreboardRepository.ResetScoreboard(cancellationToken);
            return new ResetScoreboardResponse();
        }
    }

    #endregion

    #region Response

    public record ResetScoreboardResponse();

    #endregion
}
