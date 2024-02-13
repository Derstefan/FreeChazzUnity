using System.Collections.Generic;
using UnityEngine;

public class Animator
{
    public List<Animation> animations;

    public int currentAnimationIndex = 0;

    public float progressTime = 0f;

    public float duration;

    public UpdateDataDTO updateData;

    public Animator(UpdateDataDTO updateData, GameManager gameManager)
    {
        this.updateData = updateData;
        this.animations = new List<Animation>();
        for (int i = 0; i < updateData.drawEvent.events.Length; i++)
        {
            //create animation for each event 
            EventDTO eventDTO = updateData.drawEvent.events[i];

            Animation animation = null;
            switch (eventDTO.type)
            {
                case "MOVE":
                    animation = new MovementAnimation(gameManager, eventDTO);
                    break;
                case "DESTROY":
                    animation = new DestroyAnimation(gameManager, eventDTO);
                    break;
                case "SWAP":
                    animation = new SwapAnimation(gameManager, eventDTO);
                    break;
                case "MOVEANDDESTROY":
                    animation = new MoveAndDestroyAnimation(gameManager, eventDTO);
                    break;
                default:
                    // animations[i] = new DestroyAnimation(piece);

                    //TODO: other cases
                    break;
            }
            if (animation != null)
            {
                duration += animation.duration;
                animations.Add(animation);
            }


        }

        Debug.Log("start animation with " + duration);
    }

    public bool animate(float delta)
    {

        Animation animation = animations[currentAnimationIndex];

        animation.progressTime += delta;
        progressTime += delta;

        if (animation.progressTime <= animation.duration)
        {
            animation.animate();
        }
        else
        {
            animation.finish();
            currentAnimationIndex++;
            if (currentAnimationIndex >= animations.Count)
            {
                Debug.Log(progressTime + " time");
                return true; //Animations finished
            }
            animations[currentAnimationIndex].progressTime += animation.progressTime - animation.duration;
        }



        return false; // not finished
    }
}
