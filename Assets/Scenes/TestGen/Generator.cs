using Assets.Scenes.Match.drawer;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public GameObject pieceView;

    private GameObject generatedPiece;
    private float size = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        generatedPiece = PieceRenderer.createPieceObject(Util.GenerateRandomString(2), new Vector3(0, 0, -10), Util.GenerateRandomString(20), 2);
        generatedPiece.transform.parent = pieceView.transform;
        generatedPiece.transform.localPosition = Vector3.zero;
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            DestroyDrawer.startDestroyAnimation(generatedPiece.gameObject, 8.3f, 1.0f, size);
            Destroy(generatedPiece);
            generatedPiece = PieceRenderer.createPieceObject(Util.GenerateRandomString(2), new Vector3(0, 0, -10), Util.GenerateRandomString(20), 2);
            generatedPiece.transform.parent = pieceView.transform;
            generatedPiece.transform.localPosition = Vector3.zero;

        }
    }


}
