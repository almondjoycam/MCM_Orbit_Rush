using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] Item[] items;
    [SerializeField] float itemSpawnInterval;
    float itemSpawnTime;
    [SerializeField] int totalItemCount = 5;
    int currentItemCount = 0;

    Item lastItemSpawned;

    void Start()
    {

    }

    void Update()
    {
        itemSpawnTime += Time.deltaTime;
        if (itemSpawnTime >= itemSpawnInterval && currentItemCount < totalItemCount)
        {
            CreateItem();
            itemSpawnTime = 0;
            currentItemCount++;
        }
    }

    void CreateItem()
    {
        int randItem = Random.Range(0, items.Length);
        lastItemSpawned = Instantiate(items[randItem], transform);
        lastItemSpawned.transform.position = Vector2.right * 9 + Random.insideUnitCircle;
    }
}
