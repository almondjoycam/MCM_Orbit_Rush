using UnityEngine;

// This item restores the player's health.
// It is meant to be rarer than normal collectibles.
public class HealthCore : Item
{
    [Header("Health Core Settings")]
    public float healthAmount = 30f;

    public override void Use(GameObject player)
    {
        // Finds the player's stats so we can heal them.
        PlayerControls playerStats = player.GetComponent<PlayerControls>();

        if (playerStats != null)
        {
            // Adds health back to the player.
            playerStats.RestoreHealth(healthAmount);

            Debug.Log("Health Core collected! Health restored by " + healthAmount);
        }
        else
        {
            // This helps catch setup mistakes in Unity.
            Debug.LogWarning("PlayerControls script not found on Player.");
        }
    }
}
