using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;


//NOTES
//This code requires asstes in game to function properly.
//This code also requires Ink to function and the Ink add-on in Unity.
//For context, it helps with dialogue.
//To download Ink, go to https://www.inklestudios.com/ink/


public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;

    private static DialogueManager instance;

    public bool dialogueisPlaying { get; private set; }
    private bool isTyping = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene.");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueisPlaying = false;
        dialoguePanel.SetActive(true);
        dialogueText.text = "";
    }

    private void Update()
    {
        if (!dialogueisPlaying)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        Debug.Log("Entering Dialogue Mode");
        currentStory = new Story(inkJSON.text);
        dialogueisPlaying = true;
        ContinueStory();
    }


    private void ExitDialogueMode()
    {
        Debug.Log("Exiting Dialogue Mode");
        dialogueisPlaying = false;
        dialogueText.text = "";
    }


    private void ContinueStory()
    {
        if (isTyping) return;
        if (currentStory.canContinue)
        {
            StopAllCoroutines();
            // Start the typewriter effect
            StartCoroutine(TypewriterEffect(currentStory.Continue()));
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private IEnumerator TypewriterEffect(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(0.01f); // Wait before showing the next letter
        }
        isTyping = false;
    }
}