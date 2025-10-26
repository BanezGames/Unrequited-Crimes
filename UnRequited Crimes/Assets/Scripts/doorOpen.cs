using UnityEngine;

public class doorOpen : baseInteractable
{
    private bool isOpening = false;
    [SerializeField] [Range(1.0f, 360.0f)] float rotationSpeed;
    [SerializeField] bool isLocked = false;
    [SerializeField] itemData requiredKey;
    [SerializeField] Transform doorTransform;
    private bool finishedMoving = true;
    private float currentAngle = 0.0f;

    protected override void Interact(playerController player, string itemName = "")
    {
        if (isLocked)
        {
            if (inventoryManager.instance.GetCurrentItemName() != requiredKey.itemName)//(itemName != requiredKey.itemName)
                return;

            inventoryManager.instance.RemoveItem(requiredKey.itemName);
            isLocked = false;
        }
        if (finishedMoving)
        {
            isOpening = true;
            finishedMoving = false;
            if (isOpening)
                Debug.Log("Opening Door");
            else
                Debug.Log("Closing Door");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening)
        {
            if (currentAngle < 120.0f)
            {
                doorTransform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
                currentAngle += Time.deltaTime * rotationSpeed;
            }
            else
                finishedMoving = true;
        }
        else
        {
            if (currentAngle > 0.0f)
            {
                doorTransform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
                currentAngle -= Time.deltaTime * rotationSpeed;
            }
            else
                finishedMoving = true;
        }
    }
}
