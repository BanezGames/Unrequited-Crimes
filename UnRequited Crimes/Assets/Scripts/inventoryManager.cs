using UnityEngine;
using System.Collections.Generic;

public class inventoryManager : MonoBehaviour
{

    public static inventoryManager instance;
    public List<itemData> itemList { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!instance)
        {
            itemList = new List<itemData>();
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Attempts to add an item to the player's inventory
    /// Returns true if the item was added, and false if the item already exists in the player's inventory.
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns></returns>
    public bool AddItem(itemData newItem)
    {
        if (HasItem(newItem))
        {
            Debug.Log(newItem.itemName + " is already in the player's inventory.");
            return false;
        }

        itemList.Add(newItem);
        Debug.Log(newItem.itemName + " was added to the player's inventory.");
        return true;
    }

    /// <summary>
    /// Attempts to remove an item from the player's inventory.
    /// Returns true if an item was removed, and false if the item was not in the player's inventory.
    /// </summary>
    /// <param name="removedItem"></param>
    /// <returns></returns>
    public bool RemoveItem(itemData removedItem)
    {
        return RemoveItem(removedItem.itemName);
    }

    /// <summary>
    /// Attemps to remove an item from the player's inventory.
    /// Returns true if an item was removed, and false if it was not.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public bool RemoveItem(string itemName)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemName == itemList[i].itemName)
            {
                itemList.RemoveAt(i);
                Debug.Log(itemName + " was removed from the inventory.");
            }
        }
        Debug.Log(itemName + " is NOT in the player's inventory.");
        return false;
    }

    /// <summary>
    /// Returns true if the item exists in the player's inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HasItem(itemData item)
    {
        return HasItem(item.itemName);
    }

    /// <summary>
    /// Returns true if the item exists in the player's inventory.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public bool HasItem(string itemName)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemName == itemList[i].itemName)
            {
                Debug.Log(itemName + " is in the player's inventory.");
                return true;
            }

        }
        Debug.Log(itemName + " is NOT in the player's inventory.");
        return false;
    }
}
