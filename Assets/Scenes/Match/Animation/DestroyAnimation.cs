public class DestroyAnimation : Animation
{
    private Piece piece;
    private bool shatterAnimationStarted = false;

    public bool alreadyStarted = false;
    private GameManager gameManager;
    private EventDTO eventDTO;
    public DestroyAnimation(GameManager gameManager, EventDTO eventDTO)
    {
        this.gameManager = gameManager;
        this.eventDTO = eventDTO;

        duration = 0.15f;
    }

    private void init()
    {

        piece = gameManager.getPieceById(eventDTO.pieceId);
        piece.showsMoveSet = false;

    }


    public override void animate()
    {
        if (!alreadyStarted)
        {
            init();
            alreadyStarted = true;
        }
        if (!shatterAnimationStarted)
        {
            shatterAnimationStarted = true;
            DestroyDrawer.startDestroyAnimation(piece.gameObject, 0.4f, 1.5f, gameManager.size);

        }
    }

    public override void finish()
    {
        // throw new System.NotImplementedException();
    }
}
