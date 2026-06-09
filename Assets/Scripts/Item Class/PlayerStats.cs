using UnityEngine;
using System.Collections;

// This script keeps track of the player's health, boost energy, and shield.
// The item scripts use this script to change the player's stats.
public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Boost Settings")]
    public float maxBoostEnergy = 100f;
    public float currentBoostEnergy;

    [Header("Shield Settings")]
    public bool hasShield = false;

    private Coroutine shieldCoroutine;

    private void Start()
    {
        // Starts the player with full health and full boost energy.
        currentHealth = maxHealth;
        currentBoostEnergy = maxBoostEnergy;
    }

    public void RestoreHealth(float amount)
    {
        // Adds health, but does not let it go above the max health.
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Current Health: " + currentHealth);
    }

    public void RestoreBoost(float amount)
    {
        // Adds boost energy, but does not let it go above the max boost amount.
        currentBoostEnergy += amount;
        currentBoostEnergy = Mathf.Clamp(currentBoostEnergy, 0, maxBoostEnergy);

        Debug.Log("Current Boost Energy: " + currentBoostEnergy);
    }

    public void ActivateShield(float duration)
    {
        // If the player already has a shield timer running, restart it.
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }

        shieldCoroutine = StartCoroutine(ShieldTimer(duration));
    }

    private IEnumerator ShieldTimer(float duration)
    {
        // Turns the shield on.
        hasShield = true;
        Debug.Log("Shield is active.");

        // Waits for the shield time to run out.
        yield return new WaitForSeconds(duration);

        // Turns the shield off after the timer ends.
        hasShield = false;
        Debug.Log("Shield ended.");
    }

    public void TakeDamage(float damageAmount)
    {
        // If the shield is active, it blocks the hit instead of taking damage.
        if (hasShield)
        {
            hasShield = false;
            Debug.Log("Shield blocked the hit!");
            return;
        }

        // Takes health away from the player.
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player took damage. Current Health: " + currentHealth);

        // Checks if the player has run out of health.
        if (currentHealth <= 0)
        {
            Debug.Log("Player defeated.");
        }
    }
}