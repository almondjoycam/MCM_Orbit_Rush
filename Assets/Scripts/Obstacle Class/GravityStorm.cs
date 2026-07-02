using UnityEngine;

public class GravityStorm : Obstacle
{
    Vector2 baseLevelGravity;
    float baseSteerSpeed;
    PlayerControls player;

    protected override void Start()
    {
        baseLevelGravity = Physics2D.gravity;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerControls>();
            baseSteerSpeed = player.steerSpeed;
            player.steerSpeed = Random.value * 2 - 1;
            Physics2D.gravity = Random.onUnitCircle;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.steerSpeed = baseSteerSpeed;
            player = null;
            Physics2D.gravity = baseLevelGravity;
        }
    }

    public override void Hurt(PlayerControls player)
    {
        // Doesn't actually hurt the player, just messes with the physics
        return;
    }
}
