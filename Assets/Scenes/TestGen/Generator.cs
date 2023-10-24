using Assets.Scenes.Match.drawer;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public GameObject pieceView;

    private GameObject generatedPiece;

    // Start is called before the first frame update
    void Start()
    {
        generatedPiece = PieceRenderer.createPieceObject(Util.GenerateRandomString(2), new Vector3(0, 0, -10), Util.GenerateRandomString(20), 4);
        generatedPiece.transform.parent = pieceView.transform;
        generatedPiece.transform.localPosition = Vector3.zero;
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            DestroyDrawer.startDestroyAnimation(generatedPiece.gameObject, 5.4f, 1.0f);
            Destroy(generatedPiece);
            generatedPiece = PieceRenderer.createPieceObject(Util.GenerateRandomString(2), new Vector3(0, 0, -10), Util.GenerateRandomString(20), 4);
            generatedPiece.transform.parent = pieceView.transform;
            generatedPiece.transform.localPosition = Vector3.zero;

        }
    }



    private GameObject drawExample()
    {


        return PieceDrawer.generateTest("bla", new Vector3(0, 0, -1));


    }
}
