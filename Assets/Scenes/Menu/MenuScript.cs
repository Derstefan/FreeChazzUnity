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
        Button quitButton = root.Q<Button>("quit");

        startButton.clicked+=startTestGame;
        quitButton.clicked+=exitGame;
    }


    private void startTestGame()
    {
        SceneManager.LoadScene(1);
    }


    private void exitGame()
    {
        Application.Quit();
    }
}
