using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacters
{
    public GameObject character;
    public Vector3[] characterPositions;
    public string npcName;
    public Animator animator;
    public Animator faceAnimator;
    public string playingAnimation = "";
    public string[] characterAnimationsQueue;
    public string[] characterAnimationsQueueFace;

    public void Animation(string trigger, string faceTrigger)
    {
        animator.SetTrigger(trigger);
        faceAnimator.SetTrigger(faceTrigger);
    }
}
