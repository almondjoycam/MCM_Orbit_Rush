using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public float driftSpeed = 0.5f;
    private Vector2 driftDirection;

    void Start()
    {
        // Random drift direction
        driftDirection = Random.insideUnitCircle.normalized;
        // Random rotation direction
        rotationSpeed *= Random.value > 0.5f ? 1 : -1;
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        transform.Translate(driftDirection * driftSpeed * Time.deltaTime, Space.World);
    }
}
