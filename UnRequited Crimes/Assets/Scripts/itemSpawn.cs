using UnityEngine;

public class itemSpawn : MonoBehaviour
{
    [SerializeField] GameObject item;
    [SerializeField] int maxItem;
    [SerializeField] int spawnRate;
    [SerializeField] Transform[] spawnPos;

    int itemCount;

    float spawnTime;
    bool startSpawning;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning)
        {
            spawnTime = Time.deltaTime;
            if (itemCount < maxItem && spawnTime >= spawnRate)
                spawn();
        }
    }
    void spawn()
    {
        int arrayPos = Random.Range(0,spawnPos.Length);
        Instantiate(item,spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        itemCount++;
        spawnTime = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

}
