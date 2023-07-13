using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Animator
    {
        public List<Animation> animations;

        public int currentAnimationIndex = 0;

        public float progressTime = 0f;

        public PieceDTO[] pieceDTOsAfterEvent;

        public Animator(DrawEventDTO drawEventDTO, PieceDTO[] pieceDTOsAfterEvent, GameManager gameManager)
        {
        this.pieceDTOsAfterEvent = pieceDTOsAfterEvent;
        this.animations = new List<Animation>();
        for (int i = 0; i < drawEventDTO.events.Length; i++)
        {
              //create animation for each event
              EventDTO eventDTO = drawEventDTO.events[i];
            switch (eventDTO.type)
            {
                case "MOVE":
                    
                   

                    animations.Add(new MovementAnimation(gameManager.getPieceById(eventDTO.pieceId),
                        (eventDTO.fromPos.invertY().GetVector2() + new Vector2(0.5f, -0.5f)) * gameManager.size,
                        (eventDTO.toPos.invertY().GetVector2() + new Vector2(0.5f, -0.5f)) * gameManager.size));
                    break;
                case "DESTROY":
                    animations.Add(new DestroyAnimation(gameManager.getPieceById(eventDTO.pieceId)));
                    break;
                case "SWAP":
                    animations.Add(new SwapAnimation(gameManager.getPieceByPos(eventDTO.fromPos),
                        (eventDTO.fromPos.invertY().GetVector2() + new Vector2(0.5f, -0.5f)) * gameManager.size,
                        (eventDTO.toPos.invertY().GetVector2() + new Vector2(0.5f, -0.5f)) * gameManager.size,
                         gameManager.getPieceByPos(eventDTO.toPos)));
                    break;
                case "MOVEANDDESTROY":
                    animations.Add(new MoveAndDestroyAnimation(gameManager.getPieceById(eventDTO.pieceId),
                       (eventDTO.fromPos.invertY().GetVector2() + new Vector2(0.5f, -0.5f)) * gameManager.size,
                       (eventDTO.toPos.invertY().GetVector2() + new Vector2(0.5f, -0.5f)) * gameManager.size,
                       gameManager.getPieceById(eventDTO.targetPieceId)));
                    break;
                default:
                   // animations[i] = new DestroyAnimation(piece);
                    
                    //TODO: other cases
                    break;
            }

        }
       
        }

        public bool animate(float delta)
        {
            if (currentAnimationIndex >= animations.Count)
            {
                
                return true; //Animations finished
        }
            Animation animation = animations[currentAnimationIndex];
            
       

            if (animation.progressTime <= animation.duration)
            {
                animation.animate();
            } else
            {
                currentAnimationIndex++;
            }
        animation.progressTime += delta;

        return false; // not finished
        }
    }
