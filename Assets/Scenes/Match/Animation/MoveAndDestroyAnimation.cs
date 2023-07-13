using System.Collections;
using UnityEngine;


    public class MoveAndDestroyAnimation : Animation
    {
    private const float shatterAnimationStartPoint = 0.9f;
    public Transform transform;


        public Vector2 startPosition;
        public Vector2 endPosition;
        private Piece targetPiece;

        private bool shatterAnimationStarted = false;

    public MoveAndDestroyAnimation(Piece piece, Vector3 startPosition, Vector3 endPosition, Piece targetPiece)
        {
            this.transform = piece.gameObject.transform;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.targetPiece = targetPiece;
            duration = 0.15f + Vector3.Distance(startPosition, endPosition)*0.04f;
            progressTime = 0f;
        }





    public override void animate()
    {
        float progress = progressTime / duration;
        float t = Mathf.SmoothStep(0f, 1f, progress);
        transform.position = Vector3.Lerp(startPosition, endPosition, t);
        if (!shatterAnimationStarted && t> shatterAnimationStartPoint)
        {
            DestroyDrawer.startDestroyAnimation(targetPiece.gameObject, 1.4f, 3.0f);
            shatterAnimationStarted = true;
        }
    }
}
