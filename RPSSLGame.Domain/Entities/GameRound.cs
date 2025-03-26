namespace PSSLGame.Domain.Entities;

public class GameRound
{
    public GameRound(Choice choice1, Choice choice2)
    {
        Player1Choice = choice1;
        Player2Choice = choice2;
    }
    public Guid Id => Guid.NewGuid();
    public Choice Player1Choice { get; set; }
    public Choice Player2Choice { get; set; }
    public Results Result => GetResult();

    private Results GetResult()
    {
        if (Player1Choice == Player2Choice) return Results.Tie;

        var result = LosingChoices[Player1Choice].Contains(Player2Choice) ? Results.Win : Results.Lose;
        return result;
    }

    private static readonly Dictionary<Choice, Choice[]> LosingChoices = new()
    {
        { Choice.Rock, [ Choice.Scissors, Choice.Lizard ] },
        { Choice.Paper, [ Choice.Rock, Choice.Spock ] },
        { Choice.Scissors, [ Choice.Paper, Choice.Lizard ] },
        { Choice.Lizard, [ Choice.Spock, Choice.Paper ] },
        { Choice.Spock, [ Choice.Scissors, Choice.Rock ] }
    };
}

public enum Choice
{
    Rock = 1,
    Paper,
    Scissors,
    Lizard,
    Spock
}

public enum Results
{
    Win, Lose, Tie
}
