using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MatchUiScript : MonoBehaviour
{

    private GameManager gameManager;

    private UnityEngine.UIElements.Label label;
    private UnityEngine.UIElements.Label label2;

    private void OnEnable(){
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Button b1 = root.Q<Button>("menu");
        Button b2 = root.Q<Button>("back");
        Button b3 = root.Q<Button>("nextTurn");
        Button b4 = root.Q<Button>("currentTurn");

        label = root.Q<UnityEngine.UIElements.Label>("turn");
        label2 = root.Q<UnityEngine.UIElements.Label>("log");

        b1.clicked+=menu;
        b2.clicked+=back;
        b3.clicked+=nextTurn;
        b4.clicked+=jumpToCurrentTurn;
    }

    private void menu()
    {
        SceneManager.LoadScene(0);
    }

    private void back()
    {
        if (gameManager.gameState.turn > 0)
        {
            gameManager.loadTurn(gameManager.gameState.turn - 1);
            updateTurn();
        }
    }

    private void nextTurn()
    {
        if(gameManager.gameState.turn< gameManager.gameState.maxTurns)
        {
            gameManager.loadTurn(gameManager.gameState.turn + 1);
            updateTurn();
        }
    }

    private void jumpToCurrentTurn()
    {
        gameManager.loadTurn(gameManager.gameState.maxTurns);
        updateTurn();
    }

    public void updateTurn()
    {
        label.text = "Turn " + gameManager.gameState.turn + "/" + gameManager.gameState.maxTurns;
    }

    public void writeLog(string text)
    {
        label2.text = text;
    }
}
