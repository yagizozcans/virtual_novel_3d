using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour 
{
	public static DialogueSystem instance;

	public ELEMENTS elements;

	public Chapter[] chapters;

	public int whichChapterYouAt;
	public int whichDialogueYouAt;

	public int dialogueQueue = 0;

	public float textSpeed = 0.02f;

	public DialogueCharacters[] dialogueCharacters;

	public Animator blackAnimator;

	public TextMeshProUGUI chapterText;

	public Image background;
	void Awake()
	{
		Application.targetFrameRate = 120;
		instance = this;
		StartCoroutine(FirstEntrance());
	}
	void CreateCharacters()
	{
		GameObject[] sceneCharacters = GameObject.FindGameObjectsWithTag("character");

		foreach(GameObject character in sceneCharacters)
		{
			Destroy(character);
		}

		dialogueCharacters = new DialogueCharacters[chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].dialogueCharacters.Length];

		for (int i = 0; i < chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].dialogueCharacters.Length; i++)
		{
			dialogueCharacters[i] = chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].dialogueCharacters[i];
			GameObject dialogueCharacter = Instantiate(chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].dialogueCharacters[i].character, chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].dialogueCharacters[i].characterPositions[0], chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].dialogueCharacters[i].character.transform.rotation);
			dialogueCharacters[i].animator = dialogueCharacter.GetComponent<Animator>();
			dialogueCharacters[i].faceAnimator = dialogueCharacter.transform.GetChild(0).GetComponentInChildren<Animator>();
			dialogueCharacters[i].Animation(dialogueCharacters[i].characterAnimationsQueue[dialogueQueue], dialogueCharacters[i].characterAnimationsQueueFace[dialogueQueue]);
			dialogueCharacters[i].playingAnimation = dialogueCharacters[i].characterAnimationsQueue[0];
		}
	}

	IEnumerator GoToNextDialogue()
	{
		dialogueCharacters = null;
		whichDialogueYouAt += 1;
		dialogueQueue = 0;
		blackAnimator.SetTrigger("dialogue");
		yield return new WaitForSeconds(1f);
		background.sprite = chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].background;
		CreateCharacters();
		DisplayNextDialogue();
	}

	IEnumerator ChapterChange()
	{
		if(chapters.Length == whichChapterYouAt)
		{
			blackAnimator.SetTrigger("end");
			Destroy(gameObject);
		}
		whichDialogueYouAt = 0;
		dialogueQueue = 0;
		dialogueCharacters = null;
		blackAnimator.SetTrigger("chapter");
		ChapterText();
		yield return new WaitForSeconds(4f);
		CreateCharacters();
		DisplayNextDialogue();
	}

	IEnumerator FirstEntrance()
	{
		if (chapters.Length == whichChapterYouAt)
		{
			blackAnimator.SetTrigger("end");
			Destroy(gameObject);
		}
		whichDialogueYouAt = 0;
		dialogueQueue = 0;
		dialogueCharacters = null;
		ChapterText();
		blackAnimator.SetTrigger("firstentrance");
		yield return new WaitForSeconds(2f);
		CreateCharacters();
		DisplayNextDialogue();
	}


	public void ChapterText()
	{
		chapterText.text = chapters[whichChapterYouAt].chapterName;
	}

	public void DisplayNextDialogue()
	{
		if (dialogueQueue < chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].sentences.Length)
		{
			for (int i = 0; i < dialogueCharacters.Length; i++)
			{
				if (dialogueCharacters[i].characterAnimationsQueue[dialogueQueue] != dialogueCharacters[i].playingAnimation)
				{
					dialogueCharacters[i].Animation(dialogueCharacters[i].characterAnimationsQueue[dialogueQueue] , dialogueCharacters[i].characterAnimationsQueueFace[dialogueQueue]);
				}
				dialogueCharacters[i].playingAnimation = dialogueCharacters[i].characterAnimationsQueue[dialogueQueue];
			}

			if (speechText.text == targetSpeech)
			{
				if(chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].whichCharacterGoingToSpeak[dialogueQueue] == -1)
				{
					Say(chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].sentences[dialogueQueue], "Yuuji");
				}
				else
				{
					if (chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].isAddText[dialogueQueue])
					{
						SayAdd(chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].sentences[dialogueQueue], dialogueCharacters[chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].whichCharacterGoingToSpeak[dialogueQueue]].npcName);
					}
					else
					{
						Say(chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].sentences[dialogueQueue], dialogueCharacters[chapters[whichChapterYouAt].dialogue[whichDialogueYouAt].whichCharacterGoingToSpeak[dialogueQueue]].npcName);
					}
				}

				dialogueQueue += 1;
			}
			else
			{
				StopSpeaking();
				speechText.text = targetSpeech;
			}
		}
		else
		{
			if(whichDialogueYouAt < chapters[whichChapterYouAt].dialogue.Length-1)
			{
				StartCoroutine(GoToNextDialogue());
			}
			else
			{
				whichChapterYouAt += 1;
				StartCoroutine(ChapterChange());
			}
		}
	}

	/// <summary>
	/// Say something and show it on the speech box.
	/// </summary>
	public void Say(string speech, string speaker = "")
	{
		StopSpeaking();

		speaking = StartCoroutine(Speaking(speech, false, speaker));
	}

	/// <summary>
	/// Say something to be added to what is already on the speech box.
	/// </summary>
	public void SayAdd(string speech, string speaker = "")
	{
		StopSpeaking();

		speechText.text = targetSpeech;

		speaking = StartCoroutine(Speaking(speech, true, speaker));
	}

	public void StopSpeaking()
	{
		if (isSpeaking)
		{
			StopCoroutine(speaking);
		}
		speaking = null;
	}
		
	public bool isSpeaking {get{return speaking != null;}}
	[HideInInspector] public bool isWaitingForUserInput = false;

	public string targetSpeech = "";
	Coroutine speaking = null;
	IEnumerator Speaking(string speech, bool additive, string speaker = "")
	{
		speechPanel.SetActive(true);
		targetSpeech = speech;

		if (!additive)
			speechText.text = "";
		else
			targetSpeech = speechText.text + targetSpeech;

		speakerNameText.text = DetermineSpeaker(speaker);//temporary

		isWaitingForUserInput = false;

		while(speechText.text != targetSpeech)
		{
			speechText.text += targetSpeech[speechText.text.Length];
			yield return new WaitForSeconds(textSpeed);
		}

		//text finished
		isWaitingForUserInput = true;
		while(isWaitingForUserInput)
			yield return new WaitForEndOfFrame();

		StopSpeaking();
	}

	string DetermineSpeaker(string s)
	{
		string retVal = speakerNameText.text;//default return is the current name
		if (s != speakerNameText.text && s != "")
			retVal = (s.ToLower().Contains("narrator")) ? "" : s;

		return retVal;
	}

	[System.Serializable]
	public class ELEMENTS
	{
		/// <summary>
		/// The main panel containing all dialogue related elements on the UI
		/// </summary>
		public GameObject speechPanel;
		public TextMeshProUGUI speakerNameText;
		public TextMeshProUGUI speechText;
	}
	public GameObject speechPanel {get{return elements.speechPanel;}}
	public TextMeshProUGUI speakerNameText {get{return elements.speakerNameText;}}
	public TextMeshProUGUI speechText {get{return elements.speechText;}}
}