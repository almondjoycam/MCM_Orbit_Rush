using UnityEngine;

public class Level : MonoBehaviour
{
    public LevelData levelData;
    private int itemsCollected = 0;

    void Awake()
    {
        Physics2D.gravity = new Vector2(0, -levelData.gravityConstant);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(levelData.levelTerrain, transform);

    }

    void Update()
    {
        transform.Rotate(0, 0, levelData.rotationRate * Time.deltaTime);
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
