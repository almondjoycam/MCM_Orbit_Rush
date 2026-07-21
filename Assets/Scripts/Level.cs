using UnityEngine;
using UnityEngine.Pool;

public class Level : MonoBehaviour
{
    public LevelData levelData;
    private int itemsCollected = 0;

    [Header("Obstacle and Item Spawning")]
    [SerializeField] Obstacle[] obstacles;
    [SerializeField] float obstacleSpawnInterval;
    float obstacleSpawnTime;
    [SerializeField] Item[] items;

    private ObjectPool<Obstacle> obstaclePool;

    void Awake()
    {
        Physics2D.gravity = new Vector2(0, -levelData.gravityConstant);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
           
        if (obstacles != null && obstacles.Length > 0)
        {
            obstaclePool = new ObjectPool<Obstacle>(
                createFunc: SpawnObstacle,
                actionOnGet: ActivateObstacle,
                actionOnRelease: obj => obj.Reset(),
                actionOnDestroy: obj => Destroy(obj.gameObject),
                defaultCapacity: 10,
                maxSize: 20
            );
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, levelData.rotationRate * Time.deltaTime);

        if (obstaclePool == null || obstacles == null || obstacles.Length == 0)
            return;

        obstacleSpawnTime += Time.deltaTime;

        if (obstacleSpawnInterval > 0f &&
            obstacleSpawnTime >= obstacleSpawnInterval)
        {
            obstaclePool.Get();
            obstacleSpawnTime = 0f;
        }
    }

    Obstacle SpawnObstacle()
    {
        if (obstacles == null || obstacles.Length == 0)
            return null;

        Obstacle selectedObstacle = null;

        for (int attempts = 0; attempts < 20; attempts++)
        {
            int randomIndex = Random.Range(0, obstacles.Length);
            Obstacle candidate = obstacles[randomIndex];

            if (candidate != null &&
                Random.value <= candidate.spawnChance)
            {
                selectedObstacle = candidate;
                break;
            }
        }

        // Use the first valid obstacle as a fallback.
        if (selectedObstacle == null)
        {
            foreach (Obstacle obstacle in obstacles)
            {
                if (obstacle != null)
                {
                    selectedObstacle = obstacle;
                    break;
                }
            }
        }

        if (selectedObstacle == null)
            return null;

        // Do not make moving hazards children of the rotating Level.
        Obstacle newObstacle = Instantiate(selectedObstacle);

        newObstacle.obstaclePool = obstaclePool;
        newObstacle.gameObject.SetActive(false);

        return newObstacle;
    }

    void ActivateObstacle(Obstacle obj)
    {
        if (obj == null || Camera.main == null)
            return;

        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(
            new Vector3(
                1.1f,                         // Slightly beyond right edge
                Random.Range(0.2f, 0.8f),    // Random visible height
                0f
            )
        );

        spawnPosition.z = 0f;

        obj.transform.SetParent(null);
        obj.transform.position = spawnPosition;
        obj.transform.rotation = Quaternion.identity;
        obj.gameObject.SetActive(true);
    }
    public void CollectItem(Item item)
    {
        itemsCollected++;
        if (itemsCollected >= levelData.thresholdItemNumber)
        {
            // TODO: open the checkpoint?
            Debug.Log("Win Condition");
        }
    }
}
