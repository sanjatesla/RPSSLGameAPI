using MediatR;
using PSSLGame.Domain.Common;
using PSSLGame.Domain.Services;
using static RPSSLGame.Application.Commands.PlayGame;

namespace RPSSLGame.Application.Queries;

public static class GetRandomChoice
{
    #region Query

    public record Query : IRequest<Response>;

    #endregion

    #region Handler

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IChoiceGenerator _choiceGenerator;
        public Handler(IChoiceGenerator choiceGenerator)
        {
            _choiceGenerator = choiceGenerator;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var randomChoice = await _choiceGenerator.GenerateChoice();
            return new Response((int)randomChoice, randomChoice.ToString());
        }
    }

    #endregion

    #region Response

    public record Response(int Id, string Name);

    #endregion
}
