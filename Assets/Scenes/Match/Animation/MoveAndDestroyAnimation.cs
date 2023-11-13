using UnityEngine;


public class MoveAndDestroyAnimation : Animation
{
    private const float shatterAnimationStartPoint = 0.9f;
    public Transform transform;


    public Vector3 startPosition;
    public Vector3 endPosition;
    private Piece targetPiece;

    private bool shatterAnimationStarted = false;
    private float size;

    public MoveAndDestroyAnimation(Piece piece, Vector3 startPosition, Vector3 endPosition, Piece targetPiece, float size)
    {
        this.transform = piece.gameObject.transform;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.targetPiece = targetPiece;
        duration = 0.15f + Vector3.Distance(startPosition, endPosition) * 1.04f;
        progressTime = 0f;
        this.size = size;
    }





    public override void animate()
    {
        float progress = progressTime / duration;
        float t = Mathf.SmoothStep(0f, 60f, progress);
        transform.position = Vector3.Lerp(startPosition, endPosition, t);
        if (!shatterAnimationStarted && t > shatterAnimationStartPoint)
        {
            DestroyDrawer.startDestroyAnimation(targetPiece.gameObject, 0.4f, 1.5f, size);
            shatterAnimationStarted = true;
        }
    }
}
