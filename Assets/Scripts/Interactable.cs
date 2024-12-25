using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject currentLocation;  // The current location GameObject
    public GameObject targetLocation;   // The target location GameObject to switch to
    public GameObject requiredItemPrefab; // Prefab of the required item for interaction
    public bool requiresItem = true;    // Flag to determine if an item is required for interaction

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void OnInteract(Item item)
    {
        if (requiresItem)
        {
            if (requiredItemPrefab != null)
            {
                // Check if the player has the required item in the inventory
                if (inventoryManager.HasItemInInventory(requiredItemPrefab.name))
                {
                    ProgressInteraction(item);
                }
                else
                {
                    Debug.Log("You don't have the required item in your inventory to interact with this object.");
                }
            }
            else
            {
                Debug.LogError("Required Item Prefab is not assigned!");
            }
        }
        else
        {
            // Proceed without requiring an item
            ProgressInteraction(item);
        }
    }

    private void ProgressInteraction(Item item)
    {
        if (currentLocation != null && targetLocation != null)
        {
            // Disable the current location
            currentLocation.SetActive(false);

            // Enable the target location
            targetLocation.SetActive(true);

            Debug.Log($"Interacted with {item?.gameObject.name ?? "No Item"}. Switched from {currentLocation.name} to {targetLocation.name}");

            // If an item is provided, disable it after use
            if (item != null)
            {
                item.gameObject.SetActive(false);
                Debug.Log($"{item.gameObject.name} has been disabled.");
            }
        }
        else
        {
            Debug.LogWarning("CurrentLocation or TargetLocation is not set.");
        }
    }
}