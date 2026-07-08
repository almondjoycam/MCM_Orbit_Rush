
using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    public float floatAmplitude = 0.2f;
    public float floatFrequency = 2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Floating motion
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        // Gentle rotation
        transform.Rotate(0, 0, 30f * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add fuel logic here
            Debug.Log("Fuel collected!");
            Destroy(gameObject);
        }
    }
}
