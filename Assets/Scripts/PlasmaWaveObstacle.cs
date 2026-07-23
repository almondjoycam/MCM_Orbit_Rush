using UnityEngine;

public class PlasmaWaveObstacle : Obstacle
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Move()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    public override void Hurt(PlayerControls player)
    {
        player.TakeDamage(damageAmount);
    }
}