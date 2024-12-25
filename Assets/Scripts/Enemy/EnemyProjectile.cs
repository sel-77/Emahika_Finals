using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float growthRate = 0.1f; // Speed at which the projectile grows
    public float maxScaleMultiplier = 2f; // Maximum scale before hitting the player
    private Vector2 originalScale;
    private bool hasDamagedPlayer = false;

    void Start()
    {
        originalScale = transform.localScale; // Store the original size of the projectile
    }

    void Update()
    {
        if (!hasDamagedPlayer)
        {
            ScaleProjectile();
        }
    }

    void ScaleProjectile()
    {
        // Gradually increase the size of the projectile
        Vector2 growth = Vector2.one * growthRate * Time.deltaTime;
        transform.localScale += (Vector3)growth;

        // Check if the projectile has reached its maximum size
        if (transform.localScale.x >= originalScale.x * maxScaleMultiplier &&
            transform.localScale.y >= originalScale.y * maxScaleMultiplier)
        {
            DamagePlayer(); // Hit the player when the projectile is "too large"
        }
    }

    void DamagePlayer()
    {
        hasDamagedPlayer = true;

        // Find the player and apply damage
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerModeController playerController = player.GetComponent<PlayerModeController>();
            if (playerController != null)
            {
                playerController.TakeDamage(1); // Reduce player health by 1
            }
        }
        Destroy(gameObject); // Destroy the projectile after hitting the player
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerModeController player = other.GetComponent<PlayerModeController>();
            if (player != null)
            {
                player.TakeDamage(1); // Damage the player on contact
            }
            Destroy(gameObject); // Destroy the projectile
        }
        else if (other.CompareTag("PlayerBullet")) // Assuming player bullets can destroy projectiles
        {
            Destroy(gameObject); // Destroy the projectile if hit by the player's bullet
            Debug.Log("Projectile destroyed by player!");
        }
    }

    public void TakeDamage(float amount)
    {
        Destroy(gameObject); // Destroy the projectile if it takes damage
    }
}