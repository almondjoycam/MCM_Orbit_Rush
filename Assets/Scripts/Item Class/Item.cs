using UnityEngine;

// This is the base class for all collectible items in the game.
// Fuel, shields, and health items will all inherit from this class.
public abstract class Item : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemName;
    public bool destroyAfterUse = true;

    [Header("Movement Settings")]
    public bool rotates = true;
    public float rotationSpeed = 50f;

    public bool moves = false;
    public float moveSpeed = 1f;
    public float moveDistance = 0.5f;

    private Vector3 startPosition;

    private void Start()
    {
        // Saves the starting position so the item can float around that spot.
        startPosition = transform.position;
    }

    private void Update()
    {
        // Makes the item slowly spin if rotation is turned on.
        if (rotates)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }

        // Makes the item float up and down if movement is turned on.
        if (moves)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks if the object touching the item is the player.
        if (collision.CompareTag("Player"))
        {
            // Calls the specific item's effect.
            // For example: restore fuel, give shield, or heal.
            Use(collision.gameObject);

            // Most collectible items should disappear after being picked up.
            if (destroyAfterUse)
            {
                Destroy(gameObject);
            }
        }
    }

    // Every item has to create its own version of Use().
    // This is what makes each item do something different.
    public abstract void Use(GameObject player);
}
