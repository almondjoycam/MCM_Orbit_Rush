using UnityEngine;

public class MeteorShower : Obstacle
{
    [Header("Meteor Shower Settings")]
    [SerializeField]
    int meteorCount;
    public float meteorAngle;

    GameObject meteor;
    int activeChildCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        // The shower is top-level and does not rotate with the planet.
        // transform.SetParent(null);

        meteorAngle *= Mathf.Deg2Rad;
        meteor = transform.GetChild(0).gameObject;
        if (transform.childCount < meteorCount)
        {
            int newObjectCount = meteorCount - transform.childCount;
            for (int i = 0; i < newObjectCount; i++)
            {
                Instantiate(meteor, transform);
            }
        }
        else if (transform.childCount > meteorCount)
        {
            for (int i = transform.childCount - 1; i > meteorCount; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        foreach (Transform child in transform)
        {
            child.transform.position += Random.insideUnitSphere;
            child.gameObject.SetActive(true);
            activeChildCount++;
        }
    }

    protected override void Update()
    {
        if (activeChildCount <= 0)
        {
            obstaclePool.Release(this);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // do nothing!
    }

    public override void Hurt(PlayerControls player)
    {
        // The shower itself does not hurt the player.
        // Instead, this method is called from the meteor children.
        player.TakeDamage(damageAmount);
    }

    public void OnChildInactive()
    {
        activeChildCount--;
    }
}
