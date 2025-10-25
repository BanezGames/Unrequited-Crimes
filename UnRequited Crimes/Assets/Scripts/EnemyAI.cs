using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] int HP;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootrate;

    Color colorOrig;

    float shootTimer;

    bool playerInRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        colorOrig = model.material.color;
        gameManager.instance.updateGameGoal(1);


    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (playerInRange)
        {
            if (shootTimer > shootrate)
            {
                shoot();
            }
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }

    }

    void shoot()
    {
        shootTimer = 0;

        Instantiate(bullet, shootPos.position, transform.rotation);
        
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.updateGameGoal(-1);
        }
        else 
        {
           StartCoroutine( flashRed());
        }
    }
    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
