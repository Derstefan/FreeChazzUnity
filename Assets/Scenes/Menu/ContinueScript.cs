using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ContinueScript : MonoBehaviour
{
    private VisualElement root;
    private VisualElement matchList;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        matchList = root.Q<VisualElement>("matchList");

        Button menuButton = root.Q<Button>("menu");

        menuButton.clicked += () => SceneManager.LoadScene("Menu");
        StartCoroutine(GameService.getAllMatchesOfUser(FillListWithData));
        Debug.Log("ContinueScript");
    }

    void FillListWithData(MatchDataCollection matchDataCollection)
    {
        Debug.Log("FillListWithData");
        matchList.Clear();
        var listIds = new List<string> { };

        foreach (MatchData matchData in matchDataCollection.matchDatas)
        {
            Debug.Log("Adding " + matchData.gameId);
            Button button = new Button();
            button.style.minWidth = 400;
            //textwrap the button text
            button.style.whiteSpace = WhiteSpace.Normal;


            string enemy = matchData.user1Id == PlayerPrefs.GetString("userId") ? matchData.user2Id : matchData.user1Id;
            string playerType = matchData.user1Id == PlayerPrefs.GetString("userId") ? "P1" : "P2";
            string turn = matchData.playerTurn == playerType ? "Your turn" : "Enemy turn";



            button.text = turn + " " + matchData.user1Id + " vs " + matchData.user2Id + " Turns: " + matchData.turns + " gameId:" + matchData.gameId;

            if (matchData.playerTurn == playerType)
            {
                button.style.backgroundColor = new Color(0.769f, 0.776f, 0.588f);
            }

            if (matchData.winner != "")
            {
                button.style.backgroundColor = new Color(0.784f, 0.745f, 0.588f);
            }


            button.clicked += () =>
            {
                PlayerPrefs.SetString("gameId", matchData.gameId);
                SceneManager.LoadScene("Match");
            };
            matchList.Add(button);

        }


    }
}
