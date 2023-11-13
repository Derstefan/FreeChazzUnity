using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuScript : MonoBehaviour
{

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button playMatchButton = root.Q<Button>("playMatch");
        Button generatorButton = root.Q<Button>("generator");
        Button quitButton = root.Q<Button>("quit");

        playMatchButton.clicked += playMatch;
        generatorButton.clicked += generatorScene;
        quitButton.clicked += exitGame;
    }

    private void playMatch()
    {

        SceneManager.LoadScene("QuickMatchMenu");
    }

    private void generatorScene()
    {
        SceneManager.LoadScene("TestGen");
    }

    private void exitGame()
    {
        Application.Quit();
    }
}
