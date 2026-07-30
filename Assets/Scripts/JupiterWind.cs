using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JupiterWind : MonoBehaviour
{
    [Header("Wind")]
    [SerializeField] private float windStrength = 1.2f;
    [SerializeField] private float calmDuration = 3f;
    [SerializeField] private float windDuration = 1.5f;
    [SerializeField] private bool startToRight = true;

    private Rigidbody2D rb;
    private float timer;
    private bool windActive;
    private float direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = startToRight ? 1f : -1f;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (!windActive && timer >= calmDuration)
        {
            timer = 0f;
            windActive = true;
            direction *= -1f;

            Debug.Log(direction > 0
                ? "Jupiter wind blowing right"
                : "Jupiter wind blowing left");
        }
        else if (windActive && timer >= windDuration)
        {
            timer = 0f;
            windActive = false;

            Debug.Log("Jupiter wind stopped");
        }

        if (windActive)
        {
            rb.AddForce(
                Vector2.right * direction * windStrength,
                ForceMode2D.Force
            );
        }
    }
}