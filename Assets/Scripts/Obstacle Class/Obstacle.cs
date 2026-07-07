using UnityEngine;
using UnityEngine.Pool;

public abstract class Obstacle : MonoBehaviour
{
    [Range(0,1)] public float spawnChance = 0.5f;

    [Header("Obstacle Settings")]
    public bool canMove = false;
    public float moveSpeed = 2f;
    public int damageAmount = 10;

    protected Rigidbody2D rb;
    public ObjectPool<Obstacle> obstaclePool;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (canMove)
            Move();
    }

    protected virtual void Move()
    {
        // Example: simple horizontal drift
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControls player = other.GetComponent<PlayerControls>();
            if (player != null)
                Hurt(player);
        }
    }

    public abstract void Hurt(PlayerControls player);


    // Pooling methods
    public void Reset()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnBecameInvisible()
    {
        obstaclePool.Release(this);
    }

    protected virtual void OnDestroy()
    {
        obstaclePool.Release(this);
    }
}
