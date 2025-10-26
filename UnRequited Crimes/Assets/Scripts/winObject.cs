using UnityEngine;

public class winObject : baseInteractable
{
    [SerializeField] itemData requiredItem;
    [SerializeField] bool isLocked;
    protected override void Interact(playerController player, string itemName = "")
    {
        if (isLocked && !inventoryManager.instance.HasItem(requiredItem))
            return;

        gameManager.instance.youWin();
    }
}
