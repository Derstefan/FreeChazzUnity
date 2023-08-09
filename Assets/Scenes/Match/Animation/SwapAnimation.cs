using System.Collections;
using UnityEngine;


    public class SwapAnimation : Animation
    {
        public Transform transform;


        public Vector2 startPosition;
        public Vector2 endPosition;

        public Piece toPiece;


        //this is here because an actionchain can start before the pos is updated with piece
        public bool alreadyStarted = false;
        private GameManager gameManager;
        private EventDTO eventDTO;




        public SwapAnimation(GameManager gameManager, EventDTO eventDTO)
        {
            this.gameManager = gameManager;
            this.eventDTO = eventDTO;
        }   

        private void init()
        {
            this.transform = gameManager.getPieceByPos(eventDTO.fromPos).gameObject.transform;
            this.startPosition = (eventDTO.fromPos.invertY().GetVector2() + new Vector2(0.5f, -0.5f)) * gameManager.size;
            this.endPosition = (eventDTO.toPos.invertY().GetVector2() + new Vector2(0.5f, -0.5f)) * gameManager.size;
            this.toPiece = gameManager.getPieceByPos(eventDTO.toPos);
            duration = 0.3f + Vector3.Distance(startPosition, endPosition) * 0.08f;
            progressTime = 0f;
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
        transform.position = Vector3.Lerp(startPosition, endPosition, t);
        toPiece.gameObject.transform.position = Vector3.Lerp(endPosition, startPosition, t);
    }
}
