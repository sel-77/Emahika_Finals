using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartScreenManager : MonoBehaviour
{
    // Function to load the first game scene
    public void StartGame()
    {
        // Replace "GameScene" with the name of your first game scene
        SceneManager.LoadScene("GameScene");
    }

    // Function to quit the application
    public void QuitGame()
    {
        #if UNITY_EDITOR
        // If running in the Unity Editor, stop playing the game
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Quit the application
        Application.Quit();
        #endif
    }
}
