using UnityEngine;


public class SwapAnimation : Animation
{

    public Piece piece;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public Piece toPiece;

    //this is here because an actionchain can start before the pos is updated with piece
    public bool alreadyStarted = false;
    private GameManager gameManager;
    private EventDTO eventDTO;




    public SwapAnimation(GameManager gameManager, EventDTO eventDTO)
    {
        this.gameManager = gameManager;
        this.eventDTO = eventDTO;

        duration = 0.3f + Pos.distance(eventDTO.fromPos, eventDTO.toPos) * 0.2f;

    }

    private void init()
    {
        this.piece = gameManager.getPieceByPos(eventDTO.fromPos);
        this.toPiece = gameManager.getPieceByPos(eventDTO.toPos);
        this.startPosition = AnimationUtil.getVector3FromPos(eventDTO.fromPos, gameManager.size);
        this.endPosition = AnimationUtil.getVector3FromPos(eventDTO.toPos, gameManager.size);
    }






    public override void animate()
    {
        if (!alreadyStarted)
        {
            init();
            alreadyStarted = true;
        }
        float progress = progressTime / duration;
        float t = Mathf.SmoothStep(0f, 1f, progress);
        piece.gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, t);
        toPiece.gameObject.transform.position = Vector3.Lerp(endPosition, startPosition, t);
    }

    public override void finish()
    {
        //throw new System.NotImplementedException();
    }
}
