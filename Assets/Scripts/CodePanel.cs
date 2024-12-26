using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI codeText; // Text to display the code
    [SerializeField] private GameObject keypadCanvas; // The canvas to enable when interacting
    [SerializeField] private GameObject carouselOpened; // The opened carousel object
    [SerializeField] private GameObject carouselClosed; // The closed carousel object

    private string codeTextValue = ""; // Stores the current code input
    private bool isCodeUnlocked = false; // Tracks if the code has been guessed

    void Start()
    {
        // Ensure initial states are set
        if (carouselOpened != null) carouselOpened.SetActive(false);
        if (carouselClosed != null) carouselClosed.SetActive(true);
        if (keypadCanvas != null) keypadCanvas.SetActive(false);
    }

    void Update()
    {
        // Update the displayed code
        if (codeText != null)
        {
            codeText.text = codeTextValue;
        }

        // Close the keypad panel if the player presses ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
        }

        // Check if the entered code is correct
        if (codeTextValue == "7182" && !isCodeUnlocked)    // CODE ====================================================
        {
            UnlockCarousel();
            CloseKeypad();
        }

        // Reset the code input if it exceeds the maximum length
        if (codeTextValue.Length >= 4)
        {
            codeTextValue = "";
        }
    }

    public void AddDigit(string digit)
    {
        codeTextValue += digit; // Append the digit to the code
    }

    private void UnlockCarousel()
    {
        // Enable the opened carousel and disable the closed one
        if (carouselOpened != null) carouselOpened.SetActive(true);
        if (carouselClosed != null) carouselClosed.SetActive(false);
        isCodeUnlocked = true; // Prevent re-triggering the unlock
        Debug.Log("Carousel unlocked!");
    }

    // Method to open the keypad canvas
    public void OpenKeypad()
    {
        if (keypadCanvas != null)
        {
            keypadCanvas.SetActive(true);
            Debug.Log("Keypad opened.");
        }
        else
        {
            Debug.LogWarning("KeypadCanvas is not assigned.");
        }
    }

    // Method to close the keypad canvas
    public void CloseKeypad()
    {
        if (keypadCanvas != null)
        {
            keypadCanvas.SetActive(false);
            Debug.Log("Keypad closed.");
        }
    }
}