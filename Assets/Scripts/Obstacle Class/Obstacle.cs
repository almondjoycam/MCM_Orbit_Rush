using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public bool canMove = false;
    public float moveSpeed = 2f;
    public int damageAmount = 10;

    protected Rigidbody2D rb;

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
            PlayerStats player = other.GetComponent<PlayerStats>();
            if (player != null)
                Hurt(player);
        }
    }

    public abstract void Hurt(PlayerStats player);
}
