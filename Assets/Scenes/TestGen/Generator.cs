using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public GameObject pieceView;

    private GameObject generatedPiece;
    // Start is called before the first frame update
    void Start()
    {
        generatedPiece = PieceDrawer.generatePieceObject(Util.GenerateRandomString(2),new Vector3(0,0,-1), Util.GenerateRandomString(20),1);
        generatedPiece.transform.parent = pieceView.transform;
    }


     void Update()
    {
        if (Input.GetButtonDown("Fire1")){
            DestroyDrawer.startDestroyAnimation(generatedPiece,11.4f,3.0f);
            Destroy(generatedPiece);
            generatedPiece = PieceDrawer.generatePieceObject(Util.GenerateRandomString(2),new Vector3(0,0,-1), Util.GenerateRandomString(20),1);
        }
    }



    private GameObject drawExample(){

        
       return PieceDrawer.generateTest("bla",new Vector3(0,0,-1));


    }
}
