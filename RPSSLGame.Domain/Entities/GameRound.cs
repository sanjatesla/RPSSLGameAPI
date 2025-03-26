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

    /// <summary>Gets the result of the game.</summary>
    private Results GetResult()
    {
        // If the choices are the same, it's a tie
        if (Player1Choice == Player2Choice) return Results.Tie;

        // If the player1 choice is in the losing choices of player2, player2 wins
        var result = LosingChoices[Player1Choice].Contains(Player2Choice) ? Results.Win : Results.Lose;
        return result;
    }

    // Choices that lose against the key choice
    private static readonly Dictionary<Choices, Choices[]> LosingChoices = new()
    {
        { Choices.Rock, [ Choices.Scissors, Choices.Lizard ] },
        { Choices.Paper, [ Choices.Rock, Choices.Spock ] },
        { Choices.Scissors, [ Choices.Paper, Choices.Lizard ] },
        { Choices.Lizard, [ Choices.Spock, Choices.Paper ] },
        { Choices.Spock, [ Choices.Scissors, Choices.Rock ] }
    };
}
