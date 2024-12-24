using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{   
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite sprite;
    
    [TextArea]
    [SerializeField]
    private string itemDescription;

    private InventoryManager inventoryManager;

    // Public property to access itemName
    public string ItemName => itemName; // Use this property to access itemName

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnMouseDown()
    {
        int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
        if(leftOverItems <= 0)
            gameObject.GetComponent<Renderer>().enabled = false;
        else
            quantity = leftOverItems;
    }
}
