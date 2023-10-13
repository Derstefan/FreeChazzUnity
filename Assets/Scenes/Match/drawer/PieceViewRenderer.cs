using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PieceViewRenderer
{
    private Transform parent;
    private Transform piecePic;
    private Transform actionGrid;
    private GameObject background;

    public PieceViewRenderer(Transform parent)
    {
        this.parent = parent;
        this.piecePic = parent.Find("PiecePic").transform;
        this.actionGrid = parent.Find("ActionGrid").transform;
    }
    public void render(Piece piece,PieceTypeDTO pieceTypeDTO)
    {

        removeChilds(actionGrid);
        removeChilds(piecePic);


        PieceDrawer.render(piecePic,piece, 5);
        GridRenderer.render(actionGrid, 5, 5, pieceTypeDTO.actions);
    }

    public void hide()
    {
        removeChilds(actionGrid);
        removeChilds(piecePic);
    }


    private void removeChilds(Transform transform)
    {
        // Remove existing children of the transform
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
