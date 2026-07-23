using System.Collections;
using UnityEngine;

public class SolarFlareObstacle : Obstacle
{
    [Header("Solar Flare Timing")]
    [SerializeField] private float activeTime = 1.5f;
    [SerializeField] private float inactiveTime = 2.5f;

    [Header("References")]
    [SerializeField] private SpriteRenderer flareRenderer;
    [SerializeField] private Collider2D damageCollider;

    private Coroutine flareCycle;

    protected override void Start()
    {
        base.Start();

        if (flareRenderer == null)
            flareRenderer = GetComponentInChildren<SpriteRenderer>();

        if (damageCollider == null)
            damageCollider = GetComponentInChildren<Collider2D>();
    }

    private void OnEnable()
    {
        Debug.Log($"{name}: Solar flare enabled.");

        if (flareCycle != null)
            StopCoroutine(flareCycle);

        flareCycle = StartCoroutine(FlareCycle());
    }

    private void OnDisable()
    {
        if (flareCycle != null)
        {
            StopCoroutine(flareCycle);
            flareCycle = null;
        }
    }

    private IEnumerator FlareCycle()
    {
        while (true)
        {
            SetFlareState(false);
            yield return new WaitForSeconds(inactiveTime);

            SetFlareState(true);
            yield return new WaitForSeconds(activeTime);
        }
    }

    private void SetFlareState(bool active)
    {
        if (flareRenderer != null)
            flareRenderer.enabled = active;

        if (damageCollider != null)
            damageCollider.enabled = active;

        Debug.Log($"{name}: flare active = {active}");
    }

    protected override void Move()
    {
        // Stationary hazard.
    }

    public override void Hurt(PlayerControls player)
    {
        if (player != null)
            player.TakeDamage(damageAmount);
    }

    protected override void OnBecameInvisible()
    {
        // Hand-placed hazard: do not return it to the pool.
    }
}
