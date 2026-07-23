using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("Status Bars")]
    [SerializeField] private StatusBarFrames healthBar;
    [SerializeField] private StatusBarFrames fuelBar;
    [SerializeField] private StatusBarFrames shieldBar;

    public void SetHealth(float healthAmount)
    {
        if (healthBar != null)
        {
            healthBar.SetValue(healthAmount);
        }
    }

    public void SetFuel(float fuelAmount)
    {
        if (fuelBar != null)
        {
            fuelBar.SetValue(fuelAmount);
        }
    }

    public void SetShield(float shieldAmount)
    {
        if (shieldBar != null)
        {
            shieldBar.SetValue(shieldAmount);
        }
    }

    public void SetAllBars(float healthAmount, float fuelAmount, float shieldAmount)
    {
        SetHealth(healthAmount);
        SetFuel(fuelAmount);
        SetShield(shieldAmount);
    }
}