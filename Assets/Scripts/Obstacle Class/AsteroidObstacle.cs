using UnityEngine;

public class AsteroidObstacle : Obstacle
{
    private Vector2 driftDirection;

    protected override void Start()
    {
        base.Start();
        driftDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.5f, 0.5f)).normalized;
    }

    protected override void Move()
    {
        transform.Translate(driftDirection * moveSpeed * Time.deltaTime);
    }

    public override void Hurt(Player player)
    {
        player.TakeDamage(damageAmount);
        // Optional: add explosion effect or sound
    }
}
