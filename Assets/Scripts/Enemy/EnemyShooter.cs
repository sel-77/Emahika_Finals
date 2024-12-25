using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public float health = 3f;
    public float zigzagSpeed = 2f;
    public float zigzagAmplitude = 2f;

    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float fireRate = 1f;

    private float fireCooldown;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
        fireCooldown = fireRate;
    }

    void Update()
    {
        ZigzagMovement();
        HandleShooting();
    }

    void ZigzagMovement()
    {
        float offsetX = Mathf.Sin(Time.time * zigzagSpeed) * zigzagAmplitude;
        transform.position = new Vector3(originalPosition.x + offsetX, transform.position.y, transform.position.z);
    }

    void HandleShooting()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            FireProjectile();
            fireCooldown = fireRate;
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector3 playerPosition = FindObjectOfType<ShootMode>()?.transform.position ?? Vector3.zero;
                Vector3 direction = (playerPosition - transform.position).normalized;
                rb.velocity = direction * projectileSpeed;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("EnemyShooter taking damage: " + amount);
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}