using UnityEngine;


public class MovementAnimation : Animation
{
    public Piece piece;
    public Vector3 startPosition;
    public Vector3 endPosition;

    //this is here because an actionchain can start before the pos is updated with piece
    public bool alreadyStarted = false;
    private GameManager gameManager;
    private EventDTO eventDTO;
    public MovementAnimation(GameManager gameManager, EventDTO eventDTO)
    {
        this.gameManager = gameManager;
        this.eventDTO = eventDTO;

        duration = 0.5f + Pos.distance(eventDTO.fromPos, eventDTO.toPos) * 0.2f;

    }

    private void init()
    {
        piece = gameManager.getPieceById(eventDTO.pieceId);
        piece.showsMoveSet = false;
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
    }

    public override void finish()
    {
        // throw new System.NotImplementedException();
    }
}
