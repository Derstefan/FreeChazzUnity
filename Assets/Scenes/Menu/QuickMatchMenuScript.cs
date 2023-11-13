using Assets.Scenes.Match.Interfaces.DTOs.GameParams;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class QuickMatchMenuScript : MonoBehaviour
{
    private Button menuButton;
    private Button hotSeatButton;
    private Button botGameButton;
    private Button onlineGameButton;
    private Button automaticGameButton;
    private Button moreButton;
    private VisualElement moreContainer;
    private VisualElement seedContainer;
    private VisualElement sameContainer;
    private VisualElement publicContainer;
    private Button tinyButton;
    private Button smallButton;
    private Button mediumButton;
    private Button bigButton;
    private Toggle seedToggle;
    private TextField seedTextField;
    private Toggle sameToggle;
    private Toggle publicToggle;
    private Button startButton;

    public Color selectedColor;
    public Color unselectedColor;




    private RandomGameParams randomGameParams = new RandomGameParams();

    private void OnEnable()
    {

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        menuButton = root.Q<Button>("menu");
        hotSeatButton = root.Q<Button>("hotSeat");
        botGameButton = root.Q<Button>("botGame");
        onlineGameButton = root.Q<Button>("onlineGame");
        automaticGameButton = root.Q<Button>("automaticGame");
        moreButton = root.Q<Button>("more");
        moreContainer = root.Q<VisualElement>("moreContainer");
        seedContainer = root.Q<VisualElement>("seedContainer");
        sameContainer = root.Q<VisualElement>("sameContainer");
        publicContainer = root.Q<VisualElement>("publicContainer");
        tinyButton = root.Q<Button>("tiny");
        smallButton = root.Q<Button>("small");
        mediumButton = root.Q<Button>("medium");
        bigButton = root.Q<Button>("big");
        seedToggle = root.Q<Toggle>("seedToggle");
        seedTextField = root.Q<TextField>("seedTextField");
        sameToggle = root.Q<Toggle>("sameToggle");
        publicToggle = root.Q<Toggle>("publicToggle");
        startButton = root.Q<Button>("start");


        menuButton.clicked += OnMenuButtonClick;
        hotSeatButton.clicked += OnHotSeatButtonClick;
        botGameButton.clicked += OnBotGameButtonClick;
        onlineGameButton.clicked += OnOnlineGameButtonClick;
        automaticGameButton.clicked += OnAutomaticGameButtonClick;
        moreButton.clicked += OnMoreButtonClick;
        tinyButton.clicked += OnTinyButtonClick;
        smallButton.clicked += OnSmallButtonClick;
        mediumButton.clicked += OnMediumButtonClick;
        bigButton.clicked += OnBigButtonClick;
        seedToggle.RegisterValueChangedCallback(OnSeedToggleValueChanged);
        seedTextField.RegisterValueChangedCallback(OnSeedTextFieldValueChanged);
        sameToggle.RegisterValueChangedCallback(OnSameToggleValueChanged);
        publicToggle.RegisterValueChangedCallback(OnPublicToggleValueChanged);
        startButton.clicked += OnStartButtonClick;


        OnTinyButtonClick();
        OnBotGameButtonClick();

        randomGameParams.isPublic = true;
        randomGameParams.samePieces = true;
    }


    private void OnMenuButtonClick()
    {
        SceneManager.LoadScene("Menu");
    }


    private void OnHotSeatButtonClick()
    {
        hotSeatButton.style.backgroundColor = selectedColor;
        botGameButton.style.backgroundColor = unselectedColor;
        onlineGameButton.style.backgroundColor = unselectedColor;
        automaticGameButton.style.backgroundColor = unselectedColor;

        publicContainer.style.display = DisplayStyle.None;
        randomGameParams.isNetworkGame = false;
        randomGameParams.isBotEnemy = false;
        randomGameParams.isAutomatic = false;
    }

    private void OnBotGameButtonClick()
    {
        hotSeatButton.style.backgroundColor = unselectedColor;
        botGameButton.style.backgroundColor = selectedColor;
        onlineGameButton.style.backgroundColor = unselectedColor;
        automaticGameButton.style.backgroundColor = unselectedColor;

        publicContainer.style.display = DisplayStyle.None;
        randomGameParams.isNetworkGame = false;
        randomGameParams.isBotEnemy = true;
        randomGameParams.isAutomatic = false;
    }

    private void OnOnlineGameButtonClick()
    {
        hotSeatButton.style.backgroundColor = unselectedColor;
        botGameButton.style.backgroundColor = unselectedColor;
        onlineGameButton.style.backgroundColor = selectedColor;
        automaticGameButton.style.backgroundColor = unselectedColor;

        publicContainer.style.display = DisplayStyle.Flex;
        randomGameParams.isNetworkGame = true;
        randomGameParams.isBotEnemy = false;
        randomGameParams.isAutomatic = false;
    }

    private void OnAutomaticGameButtonClick()
    {
        hotSeatButton.style.backgroundColor = unselectedColor;
        botGameButton.style.backgroundColor = unselectedColor;
        onlineGameButton.style.backgroundColor = unselectedColor;
        automaticGameButton.style.backgroundColor = selectedColor;

        publicContainer.style.display = DisplayStyle.None;
        randomGameParams.isNetworkGame = false;
        randomGameParams.isBotEnemy = true;
        randomGameParams.isAutomatic = true;
    }

    private void OnMoreButtonClick()
    {
        moreContainer.style.display = DisplayStyle.None;
        seedContainer.style.display = DisplayStyle.Flex;
        sameContainer.style.display = DisplayStyle.Flex;

    }

    private void OnTinyButtonClick()
    {
        tinyButton.style.backgroundColor = selectedColor;
        smallButton.style.backgroundColor = unselectedColor;
        mediumButton.style.backgroundColor = unselectedColor;
        bigButton.style.backgroundColor = unselectedColor;

        randomGameParams.size = RandomGameParams.ESize.tiny;

    }

    private void OnSmallButtonClick()
    {
        tinyButton.style.backgroundColor = unselectedColor;
        smallButton.style.backgroundColor = selectedColor;
        mediumButton.style.backgroundColor = unselectedColor;
        bigButton.style.backgroundColor = unselectedColor;

        randomGameParams.size = RandomGameParams.ESize.small;

    }

    private void OnMediumButtonClick()
    {
        tinyButton.style.backgroundColor = unselectedColor;
        smallButton.style.backgroundColor = unselectedColor;
        mediumButton.style.backgroundColor = selectedColor;
        bigButton.style.backgroundColor = unselectedColor;

        randomGameParams.size = RandomGameParams.ESize.medium;

    }

    private void OnBigButtonClick()
    {
        tinyButton.style.backgroundColor = unselectedColor;
        smallButton.style.backgroundColor = unselectedColor;
        mediumButton.style.backgroundColor = unselectedColor;
        bigButton.style.backgroundColor = selectedColor;

        randomGameParams.size = RandomGameParams.ESize.big;

    }

    private void OnSeedToggleValueChanged(ChangeEvent<bool> evt)
    {
        seedTextField.style.display = seedToggle.value ? DisplayStyle.Flex : DisplayStyle.None;
        randomGameParams.withSeed = seedToggle.value;
    }

    private void OnSeedTextFieldValueChanged(ChangeEvent<string> evt)
    {
        randomGameParams.seed = long.Parse(evt.newValue); // TODO:check if it is a number
    }

    private void OnSameToggleValueChanged(ChangeEvent<bool> evt)
    {
        randomGameParams.samePieces = sameToggle.value;
    }

    private void OnPublicToggleValueChanged(ChangeEvent<bool> evt)
    {
        randomGameParams.isPublic = publicToggle.value;
    }

    private void OnStartButtonClick()
    {
        Params.randomGameParams = randomGameParams;
        SceneManager.LoadScene("Match");
    }

}
