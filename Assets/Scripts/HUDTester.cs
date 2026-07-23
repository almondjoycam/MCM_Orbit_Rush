using UnityEngine;

public class HUDTester : MonoBehaviour
{
    [Header("HUD Reference")]
    [SerializeField] private HUDManager hud;

    [Header("Test Values")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float fuel = 100f;
    [SerializeField] private float shield = 100f;

    [Header("Change Amount")]
    [SerializeField] private float changeAmount = 10f;

    private void Start()
    {
        UpdateHUD();
    }

    private void Update()
    {
        // Health test
        if (Input.GetKeyDown(KeyCode.H))
        {
            health -= changeAmount;
            health = Mathf.Clamp(health, 0f, 100f);
            UpdateHUD();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            health += changeAmount;
            health = Mathf.Clamp(health, 0f, 100f);
            UpdateHUD();
        }

        // Fuel test
        if (Input.GetKeyDown(KeyCode.F))
        {
            fuel -= changeAmount;
            fuel = Mathf.Clamp(fuel, 0f, 100f);
            UpdateHUD();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            fuel += changeAmount;
            fuel = Mathf.Clamp(fuel, 0f, 100f);
            UpdateHUD();
        }

        // Shield test
        if (Input.GetKeyDown(KeyCode.S))
        {
            shield -= changeAmount;
            shield = Mathf.Clamp(shield, 0f, 100f);
            UpdateHUD();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            shield += changeAmount;
            shield = Mathf.Clamp(shield, 0f, 100f);
            UpdateHUD();
        }
    }

    private void UpdateHUD()
    {
        if (hud != null)
        {
            hud.SetAllBars(health, fuel, shield);
        }
    }
}