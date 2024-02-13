using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class JoinMatchMenuScript : MonoBehaviour
{



    private TextField gameId;


    private void OnEnable()
    {

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button menuButton = root.Q<Button>("menu");
        Button join = root.Q<Button>("join");
        gameId = root.Q<TextField>("gameId");

        menuButton.clicked += OnMenuButtonClick;
        join.clicked += OnStartButtonClick;
    }


    private void OnMenuButtonClick()
    {
        Debug.Log("Menu button clicked");
        SceneManager.LoadScene("Menu");
    }



    private void OnStartButtonClick()
    {
        Debug.Log("Join button clicked " + gameId.value);
        PlayerPrefs.SetString("gameId", gameId.value);
        StartCoroutine(GameService.joinMatch(gameId.value, initGame));
    }

    private void initGame(string gameId)
    {
        if (Params.getMatchDataByGameId(gameId) != null)
        {
            Debug.Log("Game already part of");
            return;
        }

        StartCoroutine(GameService.getAllMatchesOfUser(startMatch));
    }

    private void startMatch(MatchDataCollection matchDataCollection)
    {
        Params.clearMatchData();
        foreach (MatchData matchData in matchDataCollection.matchDatas)
        {
            Params.addMatchData(matchData);
        }

        SceneManager.LoadScene("Match");
    }

}
