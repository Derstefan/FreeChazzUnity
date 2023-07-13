using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuScript : MonoBehaviour
{

    private void OnEnable(){
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;


        Button startButton = root.Q<Button>("start");
        Button startBotButton = root.Q<Button>("startBot");
        Button generatorButton = root.Q<Button>("generator");
        Button quitButton = root.Q<Button>("quit");

        startButton.clicked+=startTestGame;
        startBotButton.clicked+= startTestBotGame;
        generatorButton.clicked+=generatorScene;
        quitButton.clicked+=exitGame;
    }


    private void startTestGame()
    {
        Params.isHotSeat = true;
        SceneManager.LoadScene(1);
    }

    private void startTestBotGame()
    {
        Params.isHotSeat = false;
        SceneManager.LoadScene(1);
    }

    private void generatorScene()
    {
        SceneManager.LoadScene(2);
    }


    private void exitGame()
    {
        Application.Quit();
    }
}
