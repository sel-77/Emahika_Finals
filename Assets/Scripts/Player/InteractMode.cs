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
    public Texture2D keypadCursor; // Cursor sprite for keypads

    private Camera mainCamera; // Camera to convert screen space to world space
    private bool isHoveringInteractable = false;
    private bool isHoveringItem = false;
    private bool isHoveringKeypad = false;
    private Item heldItem = null; // Reference to the currently held item

    void Start()
    {
        mainCamera = Camera.main;
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
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, interactableLayer | itemLayer);

        if (hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            Item item = hit.collider.GetComponent<Item>();
            CodePanel codePanel = hit.collider.GetComponent<CodePanel>();

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
                heldItem = item;
                if (!isHoveringItem)
                {
                    SetCursor(itemCursor);
                    isHoveringItem = true;
                }
                return;
            }
            else if (codePanel != null)
            {
                if (!isHoveringKeypad)
                {
                    SetCursor(keypadCursor);
                    isHoveringKeypad = true;
                }
                return;
            }
        }

        // Reset cursors if not hovering over specific objects
        if (isHoveringItem)
        {
            SetCursor(defaultCursor);
            isHoveringItem = false;
        }
        if (isHoveringInteractable)
        {
            SetCursor(defaultCursor);
            isHoveringInteractable = false;
        }
        if (isHoveringKeypad)
        {
            SetCursor(defaultCursor);
            isHoveringKeypad = false;
        }
    }

    private void Interact()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, interactableLayer);

        if (hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            Item item = hit.collider.GetComponent<Item>();
            CodePanel codePanel = hit.collider.GetComponent<CodePanel>();

            if (interactable != null)
            {
                interactable.OnInteract(heldItem);
            }
            else if (codePanel != null)
            {
                codePanel.OpenKeypad();
            }
        }
    }

    private void SetCursor(Texture2D cursor)
    {
        Vector2 hotspot = Vector2.zero;
        Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
    }

    public void ResetCursorToDefault()
    {
        SetCursor(defaultCursor);
    }
}