using UnityEngine;

public class AsteroidObstacle : Obstacle
{
    private Vector2 driftDirection;
    // some sort of health stat here
    private int health = 3;

    [Header("Asteroid Settings")]
    [SerializeField]
    Vector2 sizeRange;
    [SerializeField]
    Sprite[] asteroidTextures;

    protected override void Start()
    {
        base.Start();
        driftDirection = Random.onUnitCircle;
        transform.localScale = Vector3.one * Random.Range(sizeRange.x, sizeRange.y);
        transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
        int randomSprite = Random.Range(0, asteroidTextures.Length - 1);
        GetComponent<SpriteRenderer>().sprite = asteroidTextures[randomSprite];
        GetComponent<PolygonCollider2D>().CreateFromSprite(asteroidTextures[randomSprite]);
    }

    protected override void Move()
    {
        transform.Translate(driftDirection * moveSpeed * Time.deltaTime);
    }

    public override void Hurt(PlayerControls player)
    {
        player.TakeDamage(damageAmount);
        // add some sort of force to the player and to the asteroid itself
    }

    // OnTriggerEnter2D override
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        // check if hit by a bullet
        if (other.gameObject.GetComponent<Bullet>() != null)
        {
            health--;
            Destroy(other.gameObject);
            if (health <= 0)
            {
                // explosion!
                Destroy(gameObject);
            }
        }
    }
}

