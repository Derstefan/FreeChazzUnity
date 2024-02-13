using UnityEngine;


public class MoveAndDestroyAnimation : Animation
{
    private const float shatterAnimationStartPoint = 0.9f; // at 90 % of animation tghe detruction starts


    private Piece piece;
    public Vector3 startPosition;
    public Vector3 endPosition;
    private Piece targetPiece;

    private bool shatterAnimationStarted = false;

    //this is here because an actionchain can start before the pos is updated with piece
    public bool alreadyStarted = false;
    private GameManager gameManager;
    private EventDTO eventDTO;

    public MoveAndDestroyAnimation(GameManager gameManager, EventDTO eventDTO)
    {
        this.gameManager = gameManager;
        this.eventDTO = eventDTO;

        duration = 0.45f + Pos.distance(eventDTO.fromPos, eventDTO.toPos) * 0.2f;

    }


    private void init()
    {


        piece = gameManager.getPieceById(eventDTO.pieceId);
        targetPiece = gameManager.getPieceById(eventDTO.targetPieceId);

        piece.showsMoveSet = false;
        targetPiece.showsMoveSet = false;

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
        float t = Mathf.SmoothStep(0f, 60f, progress);
        piece.gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, t);
        if (!shatterAnimationStarted && t > shatterAnimationStartPoint)
        {
            Debug.Log("start destroying");
            //TODO: check why can it be that the targetPiece.gameObject doesn't exist anymore
            DestroyDrawer.startDestroyAnimation(targetPiece.gameObject, 0.4f, 1.5f, gameManager.size);
            shatterAnimationStarted = true;
        }
    }

    public override void finish()
    {
        // throw new System.NotImplementedException();
    }

}
