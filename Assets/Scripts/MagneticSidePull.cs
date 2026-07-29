using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MagneticSidePull : MonoBehaviour
{
    [SerializeField] private float pullStrength = 0.15f;
    [SerializeField] private bool pullToRight = true;

    private Rigidbody2D rb;
    private float pullDirection;

    // Add this variable
    private bool inMagneticField = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pullDirection = pullToRight ? 1f : -1f;
    }

    private void FixedUpdate()
    {
        if (!inMagneticField)
            return;

        rb.AddForce(
            Vector2.right * pullDirection * pullStrength,
            ForceMode2D.Force
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MagneticZone"))
        {
            inMagneticField = true;
            Debug.Log("Entered magnetic zone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MagneticZone"))
        {
            inMagneticField = false;
            Debug.Log("Exited magnetic zone");
        }
    }
}