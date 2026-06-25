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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(levelData.levelTerrain, transform);
        Physics2D.gravity = new Vector2(0, -levelData.gravityConstant);

        obstaclePool = new ObjectPool<Obstacle>(
            createFunc: () => SpawnObstacle(),
            actionOnGet: obj => ActivateObstacle(obj),
            actionOnRelease: obj => obj.Reset(),
            actionOnDestroy: obj => Destroy(obj),
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    void Update()
    {
        transform.Rotate(0, 0, levelData.rotationRate * Time.deltaTime);
        obstacleSpawnTime += Time.deltaTime;
        if (obstacleSpawnTime >= obstacleSpawnInterval)
        {
            obstaclePool.Get();
            obstacleSpawnTime = 0;
        }
    }

    Obstacle SpawnObstacle()
    {
        int randObstacle = Random.Range(0, obstacles.Length - 1);
        Obstacle newObs = Instantiate(obstacles[randObstacle], transform);
        newObs.obstaclePool = obstaclePool;
        newObs.gameObject.SetActive(false);
        return newObs;
    }

    void ActivateObstacle(Obstacle obj)
    {
        // move it to just off the screen
        obj.transform.position = Vector3.right * 8;
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
