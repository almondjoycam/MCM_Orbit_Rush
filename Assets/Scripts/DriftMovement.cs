using UnityEngine;

public class DriftMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 driftDirection =
        new Vector2(-1f, -0.2f);

    [SerializeField] private float driftSpeed = 0.5f;

    private void Update()
    {
        transform.Translate(
            driftDirection.normalized *
            driftSpeed *
            Time.deltaTime,
            Space.World
        );
    }
}