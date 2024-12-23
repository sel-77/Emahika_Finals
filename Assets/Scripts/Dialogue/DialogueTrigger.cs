using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTES
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
    [SerializeField] private float triggerRadius = 2f;  // Radius to trigger dialogue when cursor is close

    private bool cursorInRange;

    private void Awake()
    {
        cursorInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        // Get the cursor position in world space
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if the cursor is within the trigger radius of the object
        float distanceToObject = Vector2.Distance(cursorPosition, transform.position);

        if (distanceToObject <= triggerRadius && !DialogueManager.GetInstance().dialogueisPlaying)
        {
            cursorInRange = true;
            visualCue.SetActive(true);

            // Trigger dialogue when left mouse button is clicked
            if (Input.GetMouseButtonDown(0))  // 0 corresponds to the left mouse button
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
