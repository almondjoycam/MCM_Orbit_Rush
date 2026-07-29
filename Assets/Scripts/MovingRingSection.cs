using UnityEngine;

public class MovingRingSection : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector2 movementOffset = new Vector2(0f, 2f);
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float waitTimeAtEnds = 0.5f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;

    private bool waiting;
    private float waitTimer;

    private void Start()
    {
        startPosition = transform.position;

        endPosition = startPosition +
            new Vector3(movementOffset.x, movementOffset.y, 0f);

        targetPosition = endPosition;
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

                targetPosition =
                    targetPosition == endPosition
                    ? startPosition
                    : endPosition;
            }

            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            waiting = true;
        }
    }
}