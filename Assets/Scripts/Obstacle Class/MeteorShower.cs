using System.Collections;
using UnityEngine;

public class MeteorShower : Obstacle
{
    [Header("Meteor Shower Settings")]
    [SerializeField] private int meteorCount = 10;

    [Tooltip("Direction in degrees. -90 means downward.")]
    public float meteorAngle = -90f;

    [SerializeField] private float spawnInterval = 0.6f;
    [SerializeField] private float spawnSpreadX = 5f;
    [SerializeField] private float spawnSpreadY = 0.5f;

    public float MeteorMoveSpeed => moveSpeed;

    private GameObject meteorTemplate;
    private int activeChildCount;
    private Coroutine spawnRoutine;
    private bool showerStarted;

    protected override void Start()
    {
        if (transform.childCount == 0)
        {
            Debug.LogError(
                "MeteorShower needs one Meteor child as a template.",
                this
            );

            return;
        }

        meteorTemplate = transform.GetChild(0).gameObject;

        CreateMeteorPool();

        // Keep every meteor hidden until its turn.
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        activeChildCount = 0;
        showerStarted = true;
        spawnRoutine = StartCoroutine(SpawnMeteors());
    }

    private void CreateMeteorPool()
    {
        while (transform.childCount < meteorCount)
        {
            Instantiate(meteorTemplate, transform);
        }

        while (transform.childCount > meteorCount)
        {
            DestroyImmediate(
                transform.GetChild(transform.childCount - 1).gameObject
            );
        }
    }

    private IEnumerator SpawnMeteors()
    {
        foreach (Transform child in transform)
        {
            child.position = GetRandomSpawnPosition();
            child.gameObject.SetActive(true);

            activeChildCount++;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return transform.position + new Vector3(
            Random.Range(-spawnSpreadX, spawnSpreadX),
            Random.Range(-spawnSpreadY, spawnSpreadY),
            0f
        );
    }

    protected override void Update()
    {
        if (!showerStarted)
            return;

        if (spawnRoutine != null)
            return;

        if (activeChildCount > 0)
            return;

        if (obstaclePool != null)
        {
            obstaclePool.Release(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // The MeteorShower parent does not damage the player.
    }

    public override void Hurt(PlayerControls player)
    {
        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }

    public void OnChildInactive()
    {
        activeChildCount = Mathf.Max(0, activeChildCount - 1);
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        showerStarted = false;
        activeChildCount = 0;
    }
}