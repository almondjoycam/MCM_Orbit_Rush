
using UnityEngine;
using System.Collections;

public class SolarFlareObstacle : Obstacle
{
    [Header("Solar Flare Timing")]
    public float activeTime = 1.5f;
    public float inactiveTime = 2f;

    private Collider2D flareCollider;
    private SpriteRenderer flareRenderer;

    protected override void Start()
    {
        base.Start();

        flareCollider = GetComponent<Collider2D>();
        flareRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(FlareCycle());
    }

    protected override void Move()
    {
        // Solar flare does not move
    }

    private IEnumerator FlareCycle()
    {
        while (true)
        {
            SetFlareActive(true);
            yield return new WaitForSeconds(activeTime);

            SetFlareActive(false);
            yield return new WaitForSeconds(inactiveTime);
        }
    }

    private void SetFlareActive(bool isActive)
    {
        if (flareCollider != null)
            flareCollider.enabled = isActive;

        if (flareRenderer != null)
            flareRenderer.enabled = isActive;
    }

    public override void Hurt(PlayerControls player)
    {
        player.TakeDamage(damageAmount);
    }
}
