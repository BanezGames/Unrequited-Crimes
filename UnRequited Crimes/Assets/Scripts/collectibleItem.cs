using UnityEngine;

public class collectibleItem : baseInteractable
{
    [SerializeField] itemData itemToPickUp;
    //[SerializeField] itemData requiredItem;

    protected override void Interact(playerController player, string itemName = "")
    {
        if (!inventoryManager.instance)
            return;
        if (inventoryManager.instance.AddItem(itemToPickUp))
            Destroy(this);
    }
}
