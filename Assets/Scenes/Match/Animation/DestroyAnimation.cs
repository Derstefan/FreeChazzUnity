using System.Collections;
using UnityEngine;


    public class DestroyAnimation : Animation
    {
        private Piece piece;
        private bool shatterAnimationStarted = false;
        public DestroyAnimation(Piece piece)
        {
            duration = 0.0f;
            progressTime = 0f;
            this.piece = piece;
        }




    public override void animate()
    {
        if (!shatterAnimationStarted)
        {
            DestroyDrawer.startDestroyAnimation(piece.gameObject, 1.4f, 3.0f);
            shatterAnimationStarted = true;
        }
    }
}
