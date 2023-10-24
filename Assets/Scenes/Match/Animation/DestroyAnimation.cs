using System;
using UnityEngine;

public class DestroyAnimation : Animation
{
    private Piece piece;
    private bool shatterAnimationStarted = false;
    public DestroyAnimation(Piece piece)
    {

        duration = 0.25f;
        progressTime = 0f;
        this.piece = piece;
    }




    public override void animate()
    {
        if (!shatterAnimationStarted)
        {
            Sprite sprite = piece.gameObject.GetComponent<SpriteRenderer>().sprite;


            Mesh mesh = new Mesh();
            mesh.SetVertices(Array.ConvertAll(sprite.vertices, i => (Vector3)i));
            mesh.SetUVs(0, sprite.uv);
            mesh.SetTriangles(Array.ConvertAll(sprite.triangles, i => (int)i), 0);

            piece.gameObject.AddComponent<MeshFilter>().mesh = mesh;

            DestroyDrawer.startDestroyAnimation(piece.gameObject, 0.2f, 3.0f);
            shatterAnimationStarted = true;
        }
    }
}
