using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] sentences;
    [SerializeField] string tip = "0 Is You To Speak";
    public int[] whichCharacterGoingToSpeak;
    public bool[] isAddText;
    public DialogueCharacters[] dialogueCharacters;
    public Sprite background;
}
