using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        Hide();
    }

    public void Reveal()
    {
        spriteRenderer.color = originalColor; // Fully visible
    }

    public void Hide()
    {
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // Fully transparent
    }
}