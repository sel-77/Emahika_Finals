using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMode : MonoBehaviour
{
    public LayerMask interactableLayer; // Layer for interactable objects
    public LayerMask itemLayer; // Layer for item objects
    public Texture2D defaultCursor; // Default cursor sprite
    public Texture2D interactCursor; // Cursor sprite for interactables
    public Texture2D itemCursor; // Cursor sprite for items

    private Camera mainCamera; // Camera to convert screen space to world space
    private bool isHoveringInteractable = false;
    private bool isHoveringItem = false;
    private Item heldItem = null; // Reference to the currently held item

    void Start()
    {
        // Automatically get the main camera
        mainCamera = Camera.main;

        // Set the default cursor at the start
        SetCursor(defaultCursor);
    }

    void Update()
    {
        CheckHoverState();

        // If left mouse button is pressed, check for interaction
        if (Input.GetMouseButtonDown(0))
        {
            Interact();
        }
    }

    private void CheckHoverState()
    {
        // Cast a ray from the mouse position to detect interactable objects
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, interactableLayer | itemLayer);

        if (hit.collider != null)
        {
            // Check if the hit object is interactable
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            Item item = hit.collider.GetComponent<Item>();
            if (interactable != null)
            {
                if (!isHoveringInteractable)
                {
                    SetCursor(interactCursor);
                    isHoveringInteractable = true;
                }
                return;
            }
            else if (item != null)
            {
                heldItem = item;  // Assign the hovered item to the heldItem variable
                if (!isHoveringItem)
                {
                    SetCursor(itemCursor);
                    isHoveringItem = true;
                }
                return;
            }
        }
        // If no item is hovered, reset to the default cursor
        if (isHoveringItem)
        {
            SetCursor(defaultCursor);
            isHoveringItem = false;
        }

        // If no interactable is hovered, reset to the default cursor
        if (isHoveringInteractable)
        {
            SetCursor(defaultCursor);
            isHoveringInteractable = false;
        }
    }

    private void Interact()
    {
        // Cast a ray from the mouse position to detect interactable objects
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, interactableLayer);

        if (hit.collider != null)
        {
            // Check if the hit object is interactable
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                // Only interact if the item is compatible
                interactable.OnInteract(heldItem);  // Pass the heldItem to the OnInteract method
            }
        }
    }

    private void SetCursor(Texture2D cursor)
    {
        Vector2 hotspot = Vector2.zero; // Adjust if necessary (e.g., center the cursor)
        Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
    }

    public void ResetCursorToDefault()
    {
        SetCursor(defaultCursor);
    }
}
