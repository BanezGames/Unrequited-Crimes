using UnityEngine;

public class baseInteractable: MonoBehaviour
{
    [SerializeField] BoxCollider interactableCollision;
    [SerializeField][Range(1.0f, 1000.0f)] int maxRange;

    /// <summary>
    /// Attempts to cause an interaction with the object.
    /// Interaction will fail if the player is not close enough
    /// What the interaction is depends on the object, and may change based on what item is held.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemName"></param>
    public void TryInteract(playerController player, string itemName = "")
    {
        if (IsInRange(player.transform.position))
            Interact(player, itemName);
    }

    protected virtual void Interact(playerController player, string itemName = "")
    {
    }

    private bool IsInRange(Vector3 playerPosition)
    {
        return ((playerPosition - transform.position).magnitude <= maxRange);
    }
}
