using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMode : MonoBehaviour
{
    public LayerMask shootableLayer; // Layer for shootable objects
    public float shootDamage = 1f; // Amount of damage dealt
    public Transform shootCrosshair; // Reference to the shoot crosshair object
    public float crosshairExpandSize = 1.5f; // Scale multiplier for crosshair expansion
    public float crosshairShrinkSpeed = 5f; // Speed of crosshair shrinking back to normal

    public float shootCooldown = 0.5f; // Time in seconds between shots (cooldown)
    private float lastShootTime = 0f; // Time when the player last shot

    private Camera mainCamera; // Camera to convert screen space to world space
    private Vector3 originalCrosshairScale; // To store the original crosshair scale
    private bool isShooting = false; // To track if the player is currently shooting

    void Start()
    {
        // Automatically get the main camera
        mainCamera = Camera.main;

        // Store the original crosshair scale
        if (shootCrosshair != null)
        {
            originalCrosshairScale = shootCrosshair.localScale;
        }
    }

    void Update()
    {
        // Only allow shooting if the cooldown has passed
        if (Time.time >= lastShootTime + shootCooldown)
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                isShooting = true;
                Shoot();
            }
        }

        // If left mouse button is released, stop shooting
        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }

        // Handle crosshair size adjustments
        HandleCrosshairScaling();
    }

    private void Shoot()
    {
        // Convert mouse position to world position
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Perform a raycast to detect all objects under the cursor
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero, Mathf.Infinity, shootableLayer);

        // Variables to track what was hit
        EnemyProjectile hitProjectile = null;
        EnemyShooter hitShooter = null;
        Enemy hitEnemy = null;

        // Loop through all hits to determine priority
        foreach (RaycastHit2D hit in hits)
        {
            if (hitProjectile == null)
            {
                hitProjectile = hit.collider.GetComponent<EnemyProjectile>();
            }

            if (hitShooter == null)
            {
                hitShooter = hit.collider.GetComponent<EnemyShooter>();
            }

            if (hitEnemy == null)
            {
                hitEnemy = hit.collider.GetComponent<Enemy>();
            }
        }

        // Prioritize hitting the projectile first
        if (hitProjectile != null)
        {
            hitProjectile.TakeDamage(shootDamage);
            Debug.Log("Shot projectile: " + hitProjectile.name);
        }
        else if (hitShooter != null) // Then prioritize the shooter
        {
            hitShooter.TakeDamage(shootDamage);
            Debug.Log("Shot enemy shooter: " + hitShooter.name);
        }
        else if (hitEnemy != null) // Finally, prioritize other enemies
        {
            hitEnemy.TakeDamage(shootDamage);
            Debug.Log("Shot enemy: " + hitEnemy.name);
        }

        // Record the time of the shot
        lastShootTime = Time.time;
    }

    private void HandleCrosshairScaling()
    {
        if (shootCrosshair == null) return;

        if (isShooting)
        {
            // Expand the crosshair
            shootCrosshair.localScale = Vector3.Lerp(
                shootCrosshair.localScale,
                originalCrosshairScale * crosshairExpandSize,
                Time.deltaTime * crosshairShrinkSpeed
            );
        }
        else
        {
            // Shrink the crosshair back to normal
            shootCrosshair.localScale = Vector3.Lerp(
                shootCrosshair.localScale,
                originalCrosshairScale,
                Time.deltaTime * crosshairShrinkSpeed
            );
        }
    }
}