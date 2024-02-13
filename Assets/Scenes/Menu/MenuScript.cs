using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuScript : MonoBehaviour
{
    private Button continueButton;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Label>("hello").text = !PlayerPrefs.HasKey("username") ? "not logged in" : "Hello " + PlayerPrefs.GetString("username");
        continueButton = root.Q<Button>("continue");
        Button playMatchButton = root.Q<Button>("playMatch");
        Button joinMatch = root.Q<Button>("joinGame");
        Button generatorButton = root.Q<Button>("generator");
        Button playgroundButton = root.Q<Button>("playground");
        Button quitButton = root.Q<Button>("quit");

        continueButton.clicked += () => continueCurrentMatch();
        playMatchButton.clicked += playMatch;
        joinMatch.clicked += () => SceneManager.LoadScene("JoinMatch");
        generatorButton.clicked += generatorScene;
        playgroundButton.clicked += playgroundScene;
        quitButton.clicked += logout;



        StartCoroutine(GameService.getAllMatchesOfUser(loadMatchData));
    }

    private void loadMatchData(MatchDataCollection matchDataCollection)
    {
        Params.clearMatchData();
        foreach (MatchData matchData in matchDataCollection.matchDatas)
        {
            Params.addMatchData(matchData);
            if (matchData.winner == "")
            {
                continueButton.style.display = DisplayStyle.Flex;
            }
        }
    }

    private void continueCurrentMatch()
    {
        SceneManager.LoadScene("Continue");
    }

    private void playMatch()
    {

        SceneManager.LoadScene("QuickMatchMenu");
    }

    private void generatorScene()
    {
        SceneManager.LoadScene("TestGen");
    }

    private void playgroundScene()
    {
        SceneManager.LoadScene("Playground");
    }

    private void logout()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Login");
    }
}
