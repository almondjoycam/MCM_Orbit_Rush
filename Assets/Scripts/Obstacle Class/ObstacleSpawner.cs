using UnityEngine;
using UnityEngine.Pool;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] Obstacle[] obstacles;
    [SerializeField] float obstacleSpawnInterval;
    float obstacleSpawnTime;

    private ObjectPool<Obstacle> obstaclePool;
    Vector3 spawnPos;

    void Start()
    {
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
        obstacleSpawnTime += Time.deltaTime;
        if (obstacleSpawnTime >= obstacleSpawnInterval)
        {
            obstaclePool.Get();
            obstacleSpawnTime = 0;
        }
    }

    public void SetSpawnPosition(Vector3 pos)
    {
        // convert to local space
        spawnPos = transform.InverseTransformVector(pos);
    }

    Obstacle SpawnObstacle()
    {
        int randObstacle;
        Obstacle newObs;
        do {
            randObstacle = Random.Range(0, obstacles.Length);
        } while (obstacles[randObstacle].spawnChance <= Random.value);
        newObs = Instantiate(obstacles[randObstacle], transform);
        newObs.obstaclePool = obstaclePool;
        newObs.gameObject.SetActive(false);
        return newObs;
    }

    void ActivateObstacle(Obstacle obj)
    {
        // move it to just off the screen
        if (obj)
        {
            obj.transform.position = spawnPos;
            obj.gameObject.SetActive(true);
        }
    }
}
