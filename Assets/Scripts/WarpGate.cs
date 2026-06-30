using UnityEngine;

public class WarpGate : MonoBehaviour
{
    public float rotationSpeed = 40f;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Warp activated!");
            // TODO: Load next level or play warp animation
        }
    }
}
