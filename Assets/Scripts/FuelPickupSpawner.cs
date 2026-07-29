using UnityEngine;

public class FuelPickupSpawner : MonoBehaviour
{
    [Header("Fuel Pickup")]
    [SerializeField] private GameObject fuelPrefab;

    [Header("Spawn Settings")]
    [SerializeField, Min(1)] private int fuelCount = 5;
    [SerializeField, Min(0f)] private float spawnRadius = 4f;

    private void Start()
    {
        if (fuelPrefab == null)
        {
            Debug.LogError(
                "FuelPickupSpawner: Fuel prefab is not assigned.",
                this
            );

            enabled = false;
            return;
        }

        SpawnFuelPickups();
    }

    private void SpawnFuelPickups()
    {
        if (Camera.main == null)
        {
            Debug.LogError("FuelPickupSpawner: Main Camera not found.", this);
            return;
        }

        Vector3 cameraCenter = Camera.main.transform.position;
        cameraCenter.z = 0f;

        for (int i = 0; i < fuelCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;

            Vector3 spawnPosition = cameraCenter +
                new Vector3(offset.x, offset.y, 0f);

            GameObject pickup = Instantiate(
                fuelPrefab,
                spawnPosition,
                Quaternion.identity
            );

            pickup.name = $"FuelCanister_{i + 1}";

            SpriteRenderer spriteRenderer =
                pickup.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = 20;
            }

            Debug.Log($"{pickup.name} spawned at {spawnPosition}");
        }
    }
}