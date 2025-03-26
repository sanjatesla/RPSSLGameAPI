using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSSLGame.Domain.Entities;

public class ScoreEntity
{
    public ScoreEntity(string player, int score = 1)
    {
        Player = player;
        Score = score;
    }
    public int Score { get; private set; } = 0;
    public string Player { get; private set; } = "";

    public void IncreaseScore(int score = 1)
    {
        Score += score;
    }
}
