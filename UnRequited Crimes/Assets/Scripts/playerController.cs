using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage //Ipickup
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpCountMax;
    [SerializeField] int gravity;

    [SerializeField] GameObject heldModel;
    [SerializeField] int shootDamage;
    [SerializeField] int shootdist;
    [SerializeField] float shootRate;


    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;
    int HPOrig;

    float shootTimer;

    bool isSprinting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootdist, Color.red);
        shootTimer += Time.deltaTime;
        movement();

        sprint();
    }
    void movement()
    {
        if (controller.isGrounded)
        {
            playerVel = Vector3.zero;
            jumpCount = 0;
        }
        else
        { 
            playerVel.y -= gravity * Time.deltaTime;
        }
        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        
        jump();
        controller.Move(playerVel * Time.deltaTime);

        if(Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            shoot();
        }

    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpCountMax)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;
        }
    }

    void shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootdist))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if(dmg != null)
            {
                dmg.takeDamage(shootDamage);
                shootTimer = 0;
            }
            Debug.Log(hit.collider.name);
            baseInteractable interactableObject = hit.collider.GetComponent<baseInteractable>();
            if (interactableObject)
            {
                // TODO:
                // When we add an inventory and items, also pass the name of the currently held item into this function
                // or pass "" if no item is currently being held
                if (interactableObject.TryInteract(this))
                    shootTimer = 0;
            }
        }

    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if(HP <= 0)
        {
            gameManager.instance.youLose();

        }
    }

    public void SwapHeldItem(itemData data)
    {
        if (data)
        {
            heldModel.GetComponent<MeshRenderer>().enabled = true;
            heldModel.GetComponent<MeshFilter>().sharedMesh = data.itemMesh;
            heldModel.GetComponent<MeshRenderer>().sharedMaterial = data.itemMaterial;
        }
        else
        {
            heldModel.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    //public void getItem(itemData data)
    //{
    //    heldModel.GetComponent<MeshFilter>().sharedMesh = data.heldModel.GetComponent<MeshFilter>().sharedMesh;
    //    heldModel.GetComponent<MeshRenderer>().sharedMaterial = data.heldModel.GetComponent<MeshRenderer>().sharedMaterial;
    //}
}
