using PSSLGame.Domain.Common;

namespace PSSLGame.Domain.Entities;

public class GameRound
{
    public GameRound(Choices choice1, Choices choice2)
    {
        Player1Choice = choice1;
        Player2Choice = choice2;
    }
    public Guid Id => Guid.NewGuid();
    public Choices Player1Choice { get; set; }
    public Choices Player2Choice { get; set; }
    public Results Result => GetResult();

    private Results GetResult()
    {
        if (Player1Choice == Player2Choice) return Results.Tie;

        var result = LosingChoices[Player1Choice].Contains(Player2Choice) ? Results.Win : Results.Lose;
        return result;
    }

    private static readonly Dictionary<Choices, Choices[]> LosingChoices = new()
    {
        { Choices.Rock, [ Choices.Scissors, Choices.Lizard ] },
        { Choices.Paper, [ Choices.Rock, Choices.Spock ] },
        { Choices.Scissors, [ Choices.Paper, Choices.Lizard ] },
        { Choices.Lizard, [ Choices.Spock, Choices.Paper ] },
        { Choices.Spock, [ Choices.Scissors, Choices.Rock ] }
    };
}
