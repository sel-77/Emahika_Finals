using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE
//This code requires assets in game to function properly.
//This code also requires Ink to function and the Ink add-on in Unity.
//For context, it helps with dialogue.
//To download Ink, go to https://www.inklestudios.com/ink/

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Trigger Area Settings")]
    [SerializeField] private float triggerRadius = 2f;

    private bool cursorInRange;

    private void Awake()
    {
        cursorInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float distanceToObject = Vector2.Distance(cursorPosition, transform.position);

        if (distanceToObject <= triggerRadius && !DialogueManager.GetInstance().dialogueisPlaying)
        {
            cursorInRange = true;
            visualCue.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            cursorInRange = false;
            visualCue.SetActive(false);
        }
    }
}
