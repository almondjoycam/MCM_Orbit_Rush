using UnityEngine;

// This item restores the player's boost energy.
// It is useful for longer jumps between orbits.
public class FuelCanister : Item
{
    [Header("Fuel Canister Settings")]
    public float boostEnergyAmount = 25f;

    public override void Use(GameObject player)
    {
        // Looks for the PlayerStats script on the player object.
        PlayerControls playerStats = player.GetComponent<PlayerControls>();

        if (playerStats != null)
        {
            // Adds boost energy back to the player.
            playerStats.RestoreBoost(boostEnergyAmount);

            Debug.Log("Fuel Canister collected! Boost energy restored by " + boostEnergyAmount);
        }
        else
        {
            // This warning helps us know if the player is missing the PlayerControls script.
            Debug.LogWarning("PlayerControls script not found on Player.");
        }
    }
}
