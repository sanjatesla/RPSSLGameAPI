using FluentValidation;
using MediatR;
using PSSLGame.Domain.Common;
using PSSLGame.Domain.Entities;
using PSSLGame.Domain.Repositories;

namespace RPSSLGame.Application.Commands;

public static class PlayGameMultiplePlayers
{
    #region Command

    public record MultiplePlayersCommand(List<Player> Players) : IRequest<MultiplePlayersResponse>;
    public record Player(string Name, Choices Choice);

    #endregion

    #region Validator
    public class CommandValidator : AbstractValidator<MultiplePlayersCommand>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Players)
                .NotNull()
                .NotEmpty().WithMessage("Players list must not be null or empty!")
                .Must(players => players.Count >= 2).WithMessage("Min number of players is 2!");

            RuleForEach(x => x.Players).ChildRules(player =>
                {
                    player.RuleFor(p => p.Name)
                        .NotNull().NotEmpty().WithMessage("Player Name is required!");
                    player.RuleFor(p => p.Choice)
                        .NotNull().IsInEnum().NotEmpty().WithMessage("Player Choice is required!");
                });
        }
    }
    #endregion

    #region Handler
    public class Handler : IRequestHandler<MultiplePlayersCommand, MultiplePlayersResponse>
    {
        private readonly IScoreboardRepository _scoreboardRepository;
        public Handler(IScoreboardRepository scoreboardRepository)
        {
            _scoreboardRepository = scoreboardRepository;
        }

        public async Task<MultiplePlayersResponse> Handle(MultiplePlayersCommand request, CancellationToken cancellationToken)
        {
            var winners = new List<Player>();
            foreach (var player in request.Players)
            {
                var wins = request.Players
                    .Where(otherPlayer => new GameRound(player.Choice, otherPlayer.Choice).Result is Results.Win)
                    .ToList();

                if (wins.Count == request.Players.Count - 1)
                {
                    winners.Add(player);
                    await _scoreboardRepository.AddScore(player.Name, cancellationToken);
                }
            }
            string result = winners.Any() ?
                $"Winners are: {string.Join(", ", winners.Select(x => x.Name))}" :
                "No winners, play again!";

            return new MultiplePlayersResponse { Winners = winners, Result = result };
        }
    }

    #endregion

    #region Response

    public record MultiplePlayersResponse
    {
        public List<Player> Winners { get; set; } = new();
        public string Result { get; set; } = string.Empty;
    }

    #endregion
}
