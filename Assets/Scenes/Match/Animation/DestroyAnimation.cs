public class DestroyAnimation : Animation
{
    private Piece piece;
    private bool shatterAnimationStarted = false;
    private float size;
    public DestroyAnimation(Piece piece, float size)
    {

        duration = 0.25f;
        progressTime = 0f;
        this.piece = piece;
        this.size = size;
    }




    public override void animate()
    {
        if (!shatterAnimationStarted)
        {
            DestroyDrawer.startDestroyAnimation(piece.gameObject, 0.4f, 1.5f, size);
            shatterAnimationStarted = true;
        }
    }
}
