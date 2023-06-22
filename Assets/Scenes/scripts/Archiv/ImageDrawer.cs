using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScriipt4 : MonoBehaviour
{


    public int width = 2048;
    public int height = 2048;
    private long seed = 0;


    public Image image;


    //experimental
    public Transform parent;
    private PieceDrawerMech pieceDrawerMech;

    private void Start()
    {
   
        Button button = GetComponent<Button>();
        button.onClick.AddListener(GenerateRandomPiece);

        //experimental
        pieceDrawerMech = new PieceDrawerMech(parent);
    }

    // Start is called before the first frame update
    void GenerateRandomPiece()
    {
        this.seed = Random.Range(0, 1000);
        Debug.Log("Generating random piece " + seed);
        PieceDrawer drawer = new PieceDrawer(width,height);
        Texture2D generatedTexture = drawer.generate(seed);

        image.sprite = Sprite.Create(generatedTexture, new Rect(0, 0, generatedTexture.width, generatedTexture.height), new Vector2(0.5f, 0.5f));


        //experimental
        pieceDrawerMech.GenerateRandomPieces(seed);
    }

}
