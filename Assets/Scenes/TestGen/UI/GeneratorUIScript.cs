using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GeneratorUIScript : MonoBehaviour
{
    private void OnEnable(){
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;


        Button b1 = root.Q<Button>("menu");


        b1.clicked+=menu;
    }

    private void menu()
    {
        SceneManager.LoadScene(0);
    }
}
