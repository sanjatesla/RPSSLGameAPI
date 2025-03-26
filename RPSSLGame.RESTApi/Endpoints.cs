using MediatR;
using Microsoft.AspNetCore.Mvc;
using RPSSLGame.Application.Commands;
using RPSSLGame.Application.Queries;

namespace RPSSLGame.RESTApi;

public static partial class Endpoints
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        app.MapPost("/play", async (PlayGame.Command request, IMediator mediator) =>
        {
            var result = await mediator.Send(request);
            return Results.Ok(result);
        })
            .WithName("PlayGame")
            .Produces<PlayGame.Response>()
            .ProducesValidationProblem();

        app.MapPost("/play-multiple", async ([FromBody] PlayGameMultiplePlayers.MultiplePlayersCommand request, IMediator mediator) =>
        {
            var result = await mediator.Send(request);
            return Results.Ok(result);
        })
            .WithName("PlayGameMultiplePlayers")
            .Produces<PlayGameMultiplePlayers.MultiplePlayersResponse>()
            .ProducesValidationProblem();

        app.MapGet("/choices", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetChoices.Query());
            return Results.Ok(result);
        })
            .WithName("GetChoices");

        app.MapGet("/choice", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetRandomChoice.Query());
            return Results.Ok(result);
        })
            .WithName("GetRandomChoice");

        app.MapGet("/scoreboard", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTopTenScoreboard.Query());
            return Results.Ok(result);
        })
            .WithName("GetScoreboard");

        app.MapPost("/reset-scoreboard", async (ResetScoreboard.ResetScoreboardCommand request, IMediator mediator) =>
        {
            var result = await mediator.Send(request);
            return Results.Ok(result);
        })
            .WithName("ResetScoreboard");
    }
}
