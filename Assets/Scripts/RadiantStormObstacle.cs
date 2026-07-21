using System.Collections;
using UnityEngine;

public class RadiantStormObstacle : Obstacle
{
    [Header("Storm Timing")]
    [SerializeField] private float warningTime = 1f;
    [SerializeField] private float activeTime = 2f;
    [SerializeField] private float inactiveTime = 3f;

    [Header("Storm Appearance")]
    [SerializeField] private SpriteRenderer stormRenderer;
    [SerializeField] private Collider2D damageCollider;

    [SerializeField, Range(0f, 1f)]
    private float warningAlpha = 0.35f;

    private Coroutine stormCycle;
    private Color originalColor;

    protected override void Start()
    {
        base.Start();

        if (stormRenderer == null)
            stormRenderer = GetComponentInChildren<SpriteRenderer>();

        if (damageCollider == null)
            damageCollider = GetComponentInChildren<Collider2D>();

        if (stormRenderer == null)
        {
            Debug.LogError($"{name}: Storm Renderer is missing.");
            enabled = false;
            return;
        }

        originalColor = stormRenderer.color;
    }

    private void OnEnable()
    {
        stormCycle = StartCoroutine(StormCycle());
    }

    private void OnDisable()
    {
        if (stormCycle != null)
        {
            StopCoroutine(stormCycle);
            stormCycle = null;
        }
    }

    private IEnumerator StormCycle()
    {
        // Wait one frame so Start() can assign the references first.
        yield return null;

        while (true)
        {
            SetInactive();
            yield return new WaitForSeconds(inactiveTime);

            SetWarning();
            yield return new WaitForSeconds(warningTime);

            SetActive();
            yield return new WaitForSeconds(activeTime);
        }
    }

    private void SetInactive()
    {
        if (stormRenderer != null)
            stormRenderer.enabled = false;

        if (damageCollider != null)
            damageCollider.enabled = false;

        Debug.Log($"{name}: storm inactive");
    }

    private void SetWarning()
    {
        if (stormRenderer != null)
        {
            Color warningColor = originalColor;
            warningColor.a = warningAlpha;

            stormRenderer.color = warningColor;
            stormRenderer.enabled = true;
        }

        // Warning is visible but still safe.
        if (damageCollider != null)
            damageCollider.enabled = false;

        Debug.Log($"{name}: storm warning");
    }

    private void SetActive()
    {
        if (stormRenderer != null)
        {
            stormRenderer.color = originalColor;
            stormRenderer.enabled = true;
        }

        if (damageCollider != null)
            damageCollider.enabled = true;

        Debug.Log($"{name}: storm active");
    }

    protected override void Move()
    {
        // Radiant storms remain stationary.
    }

    public override void Hurt(PlayerControls player)
    {
        if (player != null)
            player.TakeDamage(damageAmount);
    }

    protected override void OnBecameInvisible()
    {
        // Hand-placed hazard: do not return it to the object pool.
    }
}