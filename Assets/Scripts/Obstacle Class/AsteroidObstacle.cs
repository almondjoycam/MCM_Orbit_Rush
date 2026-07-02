using UnityEngine;

public class AsteroidObstacle : Obstacle
{
    private Vector2 driftDirection;

    protected override void Start()
    {
        base.Start();
        // Initialize drift direction or movement
        driftDirection = Random.insideUnitCircle.normalized;
    }

    protected override void Move()
    {
        // Asteroid-specific movement
        transform.Translate(driftDirection * Time.deltaTime);
    }

    public override void Hurt(PlayerStats playerStats)
    {
        // Damage the player using PlayerStats
        playerStats.TakeDamage(10f); // Adjust damage value as needed
    }
}


