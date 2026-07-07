using UnityEngine;

public class BlackHole : Obstacle
{
    [Header("Black Hole Settings")]
    public float rotSpeed = 5f;
    public float gravityPull = 5f;
    public float distanceThreshold = 0.05f;

    Rigidbody2D playerRb;
    Vector3 gravityForce;

    protected override void Update()
    {
        transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (playerRb)
        {
            gravityForce = (transform.position - playerRb.transform.position).normalized;
            playerRb.AddForce(gravityPull * gravityForce, ForceMode2D.Force);
            if (Vector3.Distance(playerRb.transform.position, transform.position) < distanceThreshold)
            {
                Hurt(playerRb.GetComponent<PlayerControls>());
                playerRb = null;
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRb = other.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRb = null;
        }
    }

    public override void Hurt(PlayerControls player)
    {
        // Instant fail!
        player.TakeDamage(player.maxHealth);
    }
}
