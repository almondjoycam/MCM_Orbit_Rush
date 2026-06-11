using UnityEngine;

public class Level : MonoBehaviour
{
    public LevelData levelData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(levelData.levelTerrain, transform);
        Physics2D.gravity = new Vector2(0, -levelData.gravityConstant);
    }

    void Update()
    {
        transform.Rotate(0, 0, levelData.rotationRate * Time.deltaTime);
    }
}
