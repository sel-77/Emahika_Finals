using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelescopeMode : MonoBehaviour
{
    public LayerMask hiddenLayer; // Layer for hidden objects
    public float revealRadius = 5f; // Radius for revealing objects
    private Camera mainCamera;

    private List<HiddenObject> currentlyRevealedObjects = new List<HiddenObject>(); // Keep track of revealed objects

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        RevealHiddenObjects();
    }

    private void RevealHiddenObjects()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Detect hidden objects within the reveal radius
        Collider2D[] hiddenObjects = Physics2D.OverlapCircleAll(mousePosition, revealRadius, hiddenLayer);

        // Convert colliders to a list of HiddenObject scripts
        List<HiddenObject> detectedObjects = new List<HiddenObject>();
        foreach (Collider2D obj in hiddenObjects)
        {
            HiddenObject hiddenObject = obj.GetComponent<HiddenObject>();
            if (hiddenObject != null)
            {
                detectedObjects.Add(hiddenObject);

                // Reveal objects not already revealed
                if (!currentlyRevealedObjects.Contains(hiddenObject))
                {
                    hiddenObject.Reveal();
                    currentlyRevealedObjects.Add(hiddenObject);
                }
            }
        }

        // Hide objects no longer within range
        for (int i = currentlyRevealedObjects.Count - 1; i >= 0; i--)
        {
            HiddenObject revealedObject = currentlyRevealedObjects[i];
            if (!detectedObjects.Contains(revealedObject))
            {
                revealedObject.Hide();
                currentlyRevealedObjects.Remove(revealedObject);
            }
        }
    }
}