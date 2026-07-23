using UnityEngine;

public class SunLevelScript : Level
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, levelData.rotationRate * Time.deltaTime);

        //if (obstaclePool == null || obstacles == null || obstacles.Length == 0)
        //    return;

        //obstacleSpawnTime += Time.deltaTime;

        //if (obstacleSpawnInterval > 0f &&
        //    obstacleSpawnTime >= obstacleSpawnInterval)
        //{
        //    obstaclePool.Get();
        //    obstacleSpawnTime = 0f;
        }
    }
}
