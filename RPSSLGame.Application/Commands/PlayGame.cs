using FluentValidation;
using MediatR;
using PSSLGame.Domain.Common;
using PSSLGame.Domain.Entities;
using PSSLGame.Domain.Repositories;
using PSSLGame.Domain.Services;

namespace RPSSLGame.Application.Commands;

public static class PlayGame
{
    #region Command

    public record Command(Choices PlayerChoice) : IRequest<Response>;

    #endregion

    #region Validator
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.PlayerChoice)
                .NotNull()
                .NotEmpty().WithMessage("Player choice is required")
                .IsInEnum();
        }
    }
    #endregion

    #region Handler
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly IChoiceGenerator _choiceGenerator;
        private readonly IScoreboardRepository _scoreboardRepository;
        private const string PLAYER = "PlayerAgainstComputer";
        private const string COMPUTER = "Computer";
        public Handler(IChoiceGenerator choiceGenerator, IScoreboardRepository scoreboardRepository)
        {
            _choiceGenerator = choiceGenerator;
            _scoreboardRepository = scoreboardRepository;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            // generate computer choice
            var computerChoice = await _choiceGenerator.GenerateChoice();

            GameRound gameRound = new(request.PlayerChoice, computerChoice);
            var result = gameRound.Result;
            
            var winner = result switch
            {
                Results.Win => PLAYER,
                Results.Lose => COMPUTER,
                _ => ""
            };
            // add score to scoreboard if not a tie
            if (!string.IsNullOrEmpty(winner))
            {
                await _scoreboardRepository.AddScore(winner, cancellationToken);
            }
            Response response = new()
            {
                Player = request.PlayerChoice.ToString(),
                Computer = computerChoice.ToString(),
                Result = result.ToString()
            };
            return response;
        }
    }

    #endregion

    #region Response

    public record Response
    {
        public string Player { get; set; } = "";
        public string Computer { get; set; } = "";
        public string Result { get; set; } = "";
    }

    #endregion
}
