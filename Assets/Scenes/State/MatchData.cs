
using System;

[Serializable]

public class MatchData
{
    public string gameId;
    public int width;
    public int height;

    public int turns;
    public int maxTurns;
    public string playerTurn;
    public string winner; // "" if not finished

    public bool p1IsBot;
    public bool p2IsBot;

    public string user1Id;
    public string user2Id;
}
