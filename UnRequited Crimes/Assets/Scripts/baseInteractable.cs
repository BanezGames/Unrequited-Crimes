using UnityEngine;

public class baseInteractable: MonoBehaviour
{
    [SerializeField] Collider interactableCollision;
    [SerializeField][Range(0.0f, 10.0f)] float maxRange;

    /// <summary>
    /// Attempts to cause an interaction with the object.
    /// Interaction will fail if the player is not close enough
    /// What the interaction is depends on the object, and may change based on what item is held.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemName"></param>
    public bool TryInteract(playerController player, string itemName = "")
    {
        if (IsInRange(player.transform.position))
        {
            Interact(player, itemName);
            return true;
        }
        return false;
    }

    protected virtual void Interact(playerController player, string itemName = "")
    {
        Debug.Log("You have interacted with this object");
    }

    private bool IsInRange(Vector3 playerPosition)
    {
        return ((playerPosition - transform.position).magnitude <= maxRange);
    }
}
