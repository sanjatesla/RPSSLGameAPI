using MediatR;
using PSSLGame.Domain.Entities;

namespace RPSSLGame.Application.Queries;

public static class GetChoices
{
    #region Query

    public record Query : IRequest<List<Response>>;

    #endregion

    #region Handler

    public class Handler : IRequestHandler<Query, List<Response>>
    {
        public Handler() { }

        public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            return Enum.GetValues<Choice>().Select(x => new Response((int)x, x.ToString())).ToList();
        }
    }

    #endregion

    #region Response

    public record Response(int Id, string Name);

    #endregion
}
