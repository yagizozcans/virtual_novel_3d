using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    public Chapter[] chapters;

    public int whichChapterYouAt;
    public int whichDialogueYouAt;

    private int dialogueQueue = 0;

    public TextMeshProUGUI speechText;
    public TextMeshProUGUI npcNameText;

    /*private void Start()
    {
        dialogueQueue = 0;
        sentences = new Queue<string>();
        StartDialogue();
    }

    public void StartDialogue()
    {
        Debug.Log("Starting dialogue with " + chapters[whichChapterYouAt].dialogue[whichChapterYouAt].dialogueCharacters[chapters[whichChapterYouAt].dialogue[whichChapterYouAt].whichCharacterGoingToSpeak[dialogueQueue]].npcName);

        sentences.Clear();

        foreach(string sentence in chapters[whichChapterYouAt].dialogue[whichChapterYouAt].sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
        npcNameText.text = chapters[whichChapterYouAt].dialogue[whichChapterYouAt].dialogueCharacters[chapters[whichChapterYouAt].dialogue[whichChapterYouAt].whichCharacterGoingToSpeak[dialogueQueue]].npcName;
        dialogueQueue += 1;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, 0.05f));
    }

    void EndDialogue()
    {
        Debug.Log("End conversation");
    }

    IEnumerator TypeSentence(string sentence,float typeSpeed)
    {
        speechText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }*/
}
