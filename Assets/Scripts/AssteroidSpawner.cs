using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public int asteroidCount = 10;
    public float spawnRadius = 5f;

    void Start()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;
            Instantiate(asteroidPrefab, spawnPos, Quaternion.identity, transform);
        }
    }
}
