public class DestroyAnimation : Animation
{
    private Piece piece;
    private bool shatterAnimationStarted = false;
    private float size = 0.3f;
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
            DestroyDrawer.startDestroyAnimation(piece.gameObject, 0.4f, 3.0f, size);
            shatterAnimationStarted = true;
        }
    }
}
