using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModeController : MonoBehaviour
{
    public enum PlayerMode { Interact, Shoot, Telescope }
    public PlayerMode currentMode = PlayerMode.Interact;

    public bool canSwitchMode = true;

    public GameObject interactCrosshair;
    public GameObject shootCrosshair;
    public GameObject telescopeCrosshair;
    public Transform crosshairTransform;

    public GameObject armWithGun;
    public Camera mainCamera;

    public int maxHealth = 3;
    public int currentHealth;
    public Image Healthbar;

    private ShootMode shooter;
    private InteractMode interactor;
    private TelescopeMode telescope;

    void Start()
    {
        interactor = interactCrosshair.GetComponent<InteractMode>();
        shooter = shootCrosshair.GetComponent<ShootMode>();
        telescope = telescopeCrosshair.GetComponent<TelescopeMode>();
        currentHealth = maxHealth;

        UpdateMode();
        UpdateUI();
    }

    void Update()
    {
        FollowCursor();

        if (canSwitchMode && Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMode();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchToTelescopeMode();
        }
    }

    private void FollowCursor()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crosshairTransform.position = new Vector3(mousePosition.x, mousePosition.y, crosshairTransform.position.z);
    }

    public void ToggleMode()
    {
        currentMode = currentMode == PlayerMode.Interact ? PlayerMode.Shoot : PlayerMode.Interact;
        UpdateMode();
    }

    private void SwitchToTelescopeMode()
    {
        currentMode = PlayerMode.Telescope;
        UpdateMode();
    }

    private void UpdateMode()
    {
        if (shooter != null) shooter.enabled = false;
        if (interactor != null) interactor.enabled = false;
        if (telescope != null) telescope.enabled = false;

        interactCrosshair.SetActive(false);
        shootCrosshair.SetActive(false);
        telescopeCrosshair.SetActive(false);

        if (currentMode == PlayerMode.Interact)
        {
            if (interactor != null) interactor.enabled = true;
            interactCrosshair.SetActive(true);

            armWithGun.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (interactor != null) interactor.ResetCursorToDefault();
        }
        else if (currentMode == PlayerMode.Shoot)
        {
            if (shooter != null) shooter.enabled = true;
            shootCrosshair.SetActive(true);

            armWithGun.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (currentMode == PlayerMode.Telescope)
        {
            if (telescope != null) telescope.enabled = true;
            telescopeCrosshair.SetActive(true);

            armWithGun.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log($"Player took {damage} damage. Current Health: {currentHealth}");
        }
        UpdateUI();
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // Add player death logic, such as restarting the level or showing a Game Over screen
        // Example: SceneManager.LoadScene("GameOver");
    }

    private void UpdateUI()
    {
        Healthbar.fillAmount = (float)currentHealth / maxHealth;
    }
}