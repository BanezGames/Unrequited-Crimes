using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerController : MonoBehaviour, IDamage //Ipickup
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] [Range(0.1f, 1.0f)] float crouchHeightMultiplier;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpCountMax;
    [SerializeField] int gravity;

    [SerializeField] GameObject heldModel;
    [SerializeField] int shootDamage;
    [SerializeField] int shootdist;
    [SerializeField] float shootRate;

    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] backgroundSounds;
    [Range(0, 1)][SerializeField] float backgroundSoundsVol;


    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;
    int HPOrig;

    float shootTimer;

    bool isSprinting;
    bool isUncrouching;
    bool isPlayingSteps;
    bool isBackgrondPlaying;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootdist, Color.red);
        shootTimer += Time.deltaTime;
        
        movement();

        sprint();
        
        if (isUncrouching)
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, Vector3.up, out hit, 1.5f))
            {
                isUncrouching = false;
                transform.localScale = new Vector3(transform.localScale.x, 1.0f, transform.localScale.z);
            }
        }
    }
    void movement()
    {
        if (controller.isGrounded)
        {
            if (moveDir.normalized.magnitude > 0.3f && !isPlayingSteps)
            {
                StartCoroutine(playStep());
                StartCoroutine(playbackground());
            }
            playerVel = Vector3.zero;
            jumpCount = 0;
        }
        else
        { 
            playerVel.y -= gravity * Time.deltaTime;
        }
        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        crouch();
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

    void crouch()
    {
        if(Input.GetButtonDown("Crouch"))
        {
            isUncrouching = false;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * crouchHeightMultiplier, transform.localScale.z);
            //playerVel.y /= crouchMod;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            isUncrouching = true;
            //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / crouchHeightMultiplier, transform.localScale.z);
            //playerVel.y *= crouchMod;
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpCountMax)
        {
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
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
            if(dmg != null && shootDamage > 0)
            {
                dmg.takeDamage(shootDamage);
                shootTimer = 0;
            }
            Debug.Log(hit.collider.name);
            baseInteractable interactableObject = hit.collider.GetComponent<baseInteractable>();
            if (interactableObject)
            {
                if (interactableObject.TryInteract(this))
                    shootTimer = 0;
            }
        }

    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
        updatePlayerUI();
        StartCoroutine( flashPlayerDmg());

        if(HP <= 0)
        {
            gameManager.instance.youLose();

        }
    }

   public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }
    IEnumerator flashPlayerDmg()
    {
        gameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageScreen.SetActive(false);
    }

    public void SwapHeldItem(itemData data)
    {
        if (data != null)
        {
            heldModel.SetActive(true);
            //heldModel.GetComponent<MeshRenderer>().enabled = true;
            heldModel.GetComponent<MeshFilter>().sharedMesh = data.itemMesh;
            heldModel.GetComponent<MeshRenderer>().sharedMaterial = data.itemMaterial;
        }
        //else
        //{
        //    heldModel.GetComponent<MeshFilter>().sharedMesh.Clear();//.SetActive(false);
        //}
    }
    
    public void ClearHeldItem()
    {
        Debug.Log("Clearing item");
        heldModel.SetActive(false);
    }

    IEnumerator playStep()
    {
        isPlayingSteps = true;
        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);
        aud.PlayOneShot(backgroundSounds[Random.Range(0, backgroundSounds.Length)], backgroundSoundsVol);

        if (isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        isPlayingSteps = false;
    }

    IEnumerator playbackground()
    {
        isBackgrondPlaying = true;
        
        aud.PlayOneShot(backgroundSounds[Random.Range(0, backgroundSounds.Length)], backgroundSoundsVol);

        if (isPlayingSteps)
        {
            yield return new WaitForSeconds(backgroundSounds.Length);
        }
        else
        {
            yield return new WaitForSeconds(backgroundSounds.Length);
        }
        isBackgrondPlaying = false;
    }
}
