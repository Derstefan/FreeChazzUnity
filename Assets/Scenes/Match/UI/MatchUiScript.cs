using Assets.Scenes.Match.drawer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MatchUiScript : MonoBehaviour
{

    private GameManager gameManager;

    private UnityEngine.UIElements.Label label;
    private UnityEngine.UIElements.Label label2;

    private VisualElement pieceView;
    private VisualElement actionView;
    private VisualElement pieceCard;
    public Sprite sprite;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Button b1 = root.Q<Button>("menu");
        Button b2 = root.Q<Button>("back");
        Button b3 = root.Q<Button>("nextTurn");
        Button b4 = root.Q<Button>("currentTurn");

        label = root.Q<UnityEngine.UIElements.Label>("turn");
        label2 = root.Q<UnityEngine.UIElements.Label>("log");


        pieceView = root.Q<VisualElement>("pieceView");
        actionView = root.Q<VisualElement>("actionView");
        pieceCard = root.Q<VisualElement>("pieceCard");


        b1.clicked += menu;
        b2.clicked += back;
        b3.clicked += nextTurn;
        b4.clicked += jumpToCurrentTurn;
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
        if (gameManager.gameState.turn < gameManager.gameState.maxTurns)
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

    public void showPiece(Piece piece, PieceTypeDTO pieceTypeDTO)
    {
        pieceCard.style.opacity = 1;
        Texture2D renderedPiece = PieceRenderer.render(pieceTypeDTO.pieceTypeId, 100, piece.owner);

        actionView.Clear();


        VisualElement piecePos = new VisualElement();
        piecePos.style.position = Position.Absolute;
        piecePos.style.left = 0 * 20 + 100;
        piecePos.style.top = (7 - 0) * 20 + 100;
        piecePos.style.width = 18;
        piecePos.style.height = 18;

        // Customize the element's appearance (e.g., setting background color)
        piecePos.style.backgroundColor = Color.green;

        // Add the element to the hierarchy (assuming you have a parent container)
        actionView.Add(piecePos);


        foreach (ActionDTO action in pieceTypeDTO.actions)
        {
            VisualElement element = new VisualElement();
            element.style.position = Position.Absolute;
            element.style.left = action.vec.x * 20 + 100;
            element.style.top = (7 - action.vec.y) * 20 + 100;
            element.style.width = 18;
            element.style.height = 18;

            // Customize the element's appearance (e.g., setting background color)
            element.style.backgroundColor = GridRenderer.getColorByType(action.type);

            // Add the element to the hierarchy (assuming you have a parent container)
            actionView.Add(element);
            // Store the element in the 2D array
        }



        pieceView.style.backgroundImage = renderedPiece;

    }


    public void hidePiece()
    {
        pieceCard.style.opacity = 0;
    }
}
