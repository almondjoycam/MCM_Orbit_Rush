using UnityEngine;

public class MovingLandingPlatform : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector2 movementOffset = new Vector2(4f, 0f);
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float waitTimeAtEnds = 0.5f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 currentTarget;

    private float waitTimer;
    private bool waiting;

    private void Start()
    {
        startPosition = transform.position;

        endPosition = startPosition +
            new Vector3(movementOffset.x, movementOffset.y, 0f);

        currentTarget = endPosition;
    }

    private void Update()
    {
        if (waiting)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtEnds)
            {
                waitTimer = 0f;
                waiting = false;

                currentTarget =
                    currentTarget == endPosition
                    ? startPosition
                    : endPosition;
            }

            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            transform.position = currentTarget;
            waiting = true;
        }
    }
}