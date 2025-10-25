using UnityEngine;

public class baseInteractable: MonoBehaviour
{
    [SerializeField] BoxCollider interactableCollision;
    [SerializeField][Range(1.0f, 1000.0f)] int maxRange;

    public virtual void Interact()
    {

    }

    public bool IsInRange()
    {
        // TODO: Check if the player is close enough to the object to interact with it
        return true;
    }
}
