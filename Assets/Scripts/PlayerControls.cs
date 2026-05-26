using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    Rigidbody2D rb;

    // Input variables
    InputAction steer;
    InputAction thrust;
    InputAction fire;
    InputAction pause;

    float steerValue;
    bool thrusting;
    bool firing;
    bool pausing;

    public int maxHealth = 10;
    int health;
    public float steerSpeed = 1;
    public float thrustPower = 1;

    // TODO: projectiles depend on the weapon, which will have polymorphism

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        steer = InputSystem.actions.FindAction("Steer");
        thrust = InputSystem.actions.FindAction("Thrust");
        fire = InputSystem.actions.FindAction("Fire");
        pause = InputSystem.actions.FindAction("Pause");
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        steerValue = steer.ReadValue<float>();
        thrusting = thrust.IsPressed();

        if (fire.IsPressed())
        {
            firing = true;
        }
        else
        {
            firing = false;
        }

        if (pause.IsPressed())
        {
            if (!pausing) Pause();
            else Resume();
        }
    }

    void FixedUpdate()
    {
        rb.AddForceX(steerValue * steerSpeed);
        if (thrusting)
        {
            rb.AddForceY(thrustPower);
        }
    }

    void Fire()
    {
        Debug.Log("Pew pew");
    }

    void Pause()
    {
        pausing = true;
        Time.timeScale = 0;
    }

    void Resume()
    {
        pausing = false;
        Time.timeScale = 1;
    }
}
