using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class inventoryManager : MonoBehaviour
{

    public static inventoryManager instance;
    private List<string> itemList;
    private int currentItemIndex;

    private List<GameObject> itemDisplay;
    [SerializeField] private GameObject itemCursor;
    [SerializeField] private GameObject inventoryBar;
    [SerializeField] private GameObject itemNameTextHolder;
    [SerializeField] private TMP_Text itemNameText;

    [SerializeField][Range(0.01f, 1.0f)] private float scrollCooldown;
    [SerializeField] [Range(0.01f, 1.0f)] private float scrollDeadzone;
    private float scrollTimer = 0.0f;
    private bool canScroll = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!instance)
        {
            itemList = new List<string>();
            itemDisplay = new List<GameObject>();
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void SetAllowedToScroll(bool scrollingAllowed)
    {
        canScroll = scrollingAllowed;
    }

    private void Update()
    {
        scrollTimer -= Time.deltaTime;
        if (scrollTimer < 0)
            scrollTimer = 0;

        if (canScroll && scrollTimer <= 0.0f)
        {
            float mouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");
            if (mouseWheel >= scrollDeadzone)
            {
                SelectNextItem();
                scrollTimer = scrollCooldown;
            }
            else if (mouseWheel <= -scrollDeadzone)
            {
                SelectPrevItem();
                scrollTimer = scrollCooldown;
            }
        }

        UpdateInventoryDisplay();
    }

    public int GetInventoryCount()
    {
        return itemList.Count;
    }

    private void SelectNextItem()
    {
        int oldIndex = currentItemIndex;
        currentItemIndex++;
        if (currentItemIndex >= itemList.Count)
            currentItemIndex = 0;
        SwitchActiveItemScale(oldIndex);
    }

    private void SelectPrevItem()
    {
        int oldIndex = currentItemIndex;
        currentItemIndex--;
        if (currentItemIndex < 0)
            currentItemIndex = itemList.Count - 1;
        SwitchActiveItemScale(oldIndex);
    }

    private void SwitchActiveItemScale(int oldIndex)
    {
        if (itemList.Count <= 0)
            return;
        itemDisplay[oldIndex].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        itemDisplay[currentItemIndex].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //itemNameTextHolder.GetComponent<TextMeshPro>().text = itemList[currentItemIndex];
        itemNameText.text = itemList[currentItemIndex];
    }

    public string GetCurrentItemName()
    {
        if (itemList.Count > 0)
            return itemList[currentItemIndex];
        else
            return "";
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

        itemList.Add(newItem.itemName);
        var newSprite = new GameObject();
        newSprite.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        var image = newSprite.AddComponent<Image>();
        image.sprite = newItem.itemDisplay;
        newSprite.transform.SetParent(inventoryBar.transform);
        itemDisplay.Add(newSprite);

        int oldIndex = currentItemIndex;
        currentItemIndex = itemList.Count - 1;
        SwitchActiveItemScale(oldIndex);

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
            if (itemName == itemList[i])
            {
                itemList.RemoveAt(i);
                Destroy(itemDisplay[i]);
                if (currentItemIndex == i)
                    currentItemIndex--;
                if (currentItemIndex < 0)
                    currentItemIndex = 0;
                SwitchActiveItemScale(currentItemIndex);
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
            if (itemName == itemList[i])
            {
                Debug.Log(itemName + " is in the player's inventory.");
                return true;
            }

        }
        Debug.Log(itemName + " is NOT in the player's inventory.");
        return false;
    }

    private void UpdateInventoryDisplay()
    {
        if (itemDisplay.Count > 0)
        {
            itemCursor.SetActive(true);
            itemCursor.transform.position = itemDisplay[currentItemIndex].transform.position;
            itemNameTextHolder.SetActive(true);
        }
        else
        {
            itemNameTextHolder.SetActive(false);
            itemCursor.SetActive(false);
        }
    }
}
