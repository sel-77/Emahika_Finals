using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 3f;
    public float growthRate = 0.5f;
    public float maxScaleMultiplier = 2f;
    private Vector2 originalScale;

    public float zigzagSpeed = 2f;
    public float zigzagAmplitude = 0.5f;

    private bool hasKilledPlayer = false;
    private Vector3 originalPosition;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    void Update()
    {
        if (!hasKilledPlayer)
        {
            Vector2 growth = Vector2.one * growthRate * Time.deltaTime;
            transform.localScale += (Vector3)growth;

            ZigzagMovement();

            if (transform.localScale.x >= originalScale.x * maxScaleMultiplier &&
                transform.localScale.y >= originalScale.y * maxScaleMultiplier)
            {
                KillPlayer();
            }
        }
    }

    void ZigzagMovement()
    {
        float offsetX = Mathf.Sin(Time.time * zigzagSpeed) * zigzagAmplitude;
        transform.position = new Vector3(originalPosition.x + offsetX, transform.position.y, transform.position.z);
    }

    public void TakeDamage(float amount)
    {
        if (!hasKilledPlayer)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void KillPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerModeController playerController = player.GetComponent<PlayerModeController>();
        playerController.TakeDamage(playerController.currentHealth);
        hasKilledPlayer = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerModeController player = collision.collider.GetComponent<PlayerModeController>();
            if (player != null)
            {
                player.TakeDamage(player.maxHealth); // Instantly kill the player
            }
        }
    }
}