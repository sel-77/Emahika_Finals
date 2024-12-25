using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmWithGun : MonoBehaviour
{
    public Transform crosshair; // Reference to the crosshair
    public float movementFactor = 0.1f; // How much the arm moves relative to the mouse
    public Vector2 screenAnchor = new Vector2(0.9f, 0.1f); // Lower-right anchor (0.9x, 0.1y)
    public Camera mainCamera;

    private Vector3 defaultPosition; // Default anchored position for the arm
    private Animator animator; // Reference to the Animator

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        // Get the Animator component
        animator = GetComponent<Animator>();

        // Calculate the default position based on screen anchor
        Vector3 screenPoint = new Vector3(
            Screen.width * screenAnchor.x,
            Screen.height * screenAnchor.y,
            mainCamera.nearClipPlane
        );
        defaultPosition = mainCamera.ScreenToWorldPoint(screenPoint);
    }

    void Update()
    {
        if (crosshair == null) return;

        // Calculate movement offset based on the mouse position
        Vector3 mouseOffset = crosshair.position - mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, mainCamera.nearClipPlane));
        mouseOffset *= movementFactor; // Scale the offset

        // Update the arm position based on the anchor and offset
        transform.position = defaultPosition + new Vector3(mouseOffset.x, mouseOffset.y, 0);

        // Trigger the shooting animation when the player shoots
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            TriggerShootAnimation();
        }
    }

    private void TriggerShootAnimation()
    {
        if (animator != null)
        {
            // Trigger the "Shoot" animation (assuming you have set this up in the Animator)
            animator.SetTrigger("Shoot");
        }
    }
}