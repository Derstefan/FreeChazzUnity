using System.Collections;
using UnityEngine;


    public class SwapAnimation : Animation
    {
        public Transform transform;


        public Vector2 startPosition;
        public Vector2 endPosition;

        public Piece toPiece;

        public SwapAnimation(Piece piece, Vector3 startPosition, Vector3 endPosition, Piece toPiece)
        {
            this.transform = piece.gameObject.transform;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.toPiece = toPiece;
            duration = 0.3f + Vector3.Distance(startPosition, endPosition)*0.08f;
            progressTime = 0f;
        }





    public override void animate()
    {
        float progress = progressTime / duration;
        float t = Mathf.SmoothStep(0f, 1f, progress);
        transform.position = Vector3.Lerp(startPosition, endPosition, t);
        toPiece.gameObject.transform.position = Vector3.Lerp(endPosition, startPosition, t);
    }
}
