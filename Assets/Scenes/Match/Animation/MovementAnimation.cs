using System.Collections;
using UnityEngine;


    public class MovementAnimation : Animation
    {
        public Transform transform;


        public Vector2 startPosition;
        public Vector2 endPosition;

        public MovementAnimation(Piece piece, Vector3 startPosition, Vector3 endPosition)
        {
            this.transform = piece.gameObject.transform;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            duration = 0.3f + Vector3.Distance(startPosition, endPosition)*0.08f;
            progressTime = 0f;
        }





    public override void animate()
    {
        float progress = progressTime / duration;
        float t = Mathf.SmoothStep(0f, 1f, progress);
        transform.position = Vector3.Lerp(startPosition, endPosition, t);
    }
}
