
using System.Collections.Generic;
using Assets.Scenes.Match.Interfaces.DTOs.GameParams;

public class Params
{
    public static RandomGameParams randomGameParams;

    public static List<MatchData> games = new List<MatchData>();

    public static float SquareSize = 1f;


    public static MatchData getMatchDataByGameId(string gameId)
    {
        foreach (MatchData matchData in games)
        {
            if (matchData.gameId == gameId)
            {
                return matchData;
            }
        }

        return null;
    }


    public static void removeMatchDataByGameId(string gameId)
    {
        MatchData matchData = getMatchDataByGameId(gameId);
        if (matchData != null)
        {
            games.Remove(matchData);
        }
    }

    public static void addMatchData(MatchData matchData)
    {
        games.Add(matchData);
    }

    public static void clearMatchData()
    {
        games.Clear();
    }

}
