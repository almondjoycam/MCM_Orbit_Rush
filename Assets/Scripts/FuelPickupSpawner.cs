using UnityEngine;

public class FuelPickupSpawner : MonoBehaviour
{
    public GameObject fuelPrefab;
    public int fuelCount = 5;
    public float spawnRadius = 4f;

    void Start()
    {
        for (int i = 0; i < fuelCount; i++)
        {
            Vector2 pos = Random.insideUnitCircle * spawnRadius;
            Instantiate(fuelPrefab, pos, Quaternion.identity, transform);
        }
    }
}
