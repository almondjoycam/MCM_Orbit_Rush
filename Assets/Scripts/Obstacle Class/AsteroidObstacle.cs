using UnityEngine;

public class AsteroidObstacle : Obstacle
{
    private Vector2 driftDirection;

    protected override void Start()
    {
        base.Start();
        // Initialize drift direction or movement
    }

    protected override void Move()
    {
        // Asteroid-specific movement
    }

    public override void Hurt(Player player)
    {
        // Damage the player
    }
}

