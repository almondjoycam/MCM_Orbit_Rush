using UnityEngine;

// This item gives the player a temporary shield.
// The shield blocks one major hit.
public class ShieldGenerator : Item
{
    [Header("Shield Generator Settings")]
    public float shieldDuration = 5f;

    public override void Use(GameObject player)
    {
        // Looks for the PlayerStats script so we can activate the shield.
        PlayerControls playerStats = player.GetComponent<PlayerControls>();

        if (playerStats != null)
        {
            // Turns on the player's shield for a short amount of time.
            playerStats.ActivateShield(shieldDuration);

            Debug.Log("Shield Generator collected! Shield activated for " + shieldDuration + " seconds.");
        }
        else
        {
            // This warning appears if the player object is not set up correctly.
            Debug.LogWarning("PlayerControls script not found on Player.");
        }
    }
}
