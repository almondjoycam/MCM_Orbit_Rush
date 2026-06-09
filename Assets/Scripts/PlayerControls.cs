using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    GameObject level;

    // Input variables
    InputAction steer;
    InputAction thrust;
    InputAction fire;
    InputAction look;
    InputAction pause;

    float steerValue;
    Vector2 lookValue;
    bool thrusting;
    bool firing;
    bool pausing;

    public int maxHealth = 10;
    int health;
    public float steerSpeed = 1;
    public float thrustPower = 1;
    public float lookSensitivity = 10;

    // TODO: projectiles depend on the weapon, which will have polymorphism
    IWeapon currentWeapon;
    [SerializeField]
    Transform aim;
    Bounds screenBounds;
    Vector3 aimDelta;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;

        steer = InputSystem.actions.FindAction("Steer");
        thrust = InputSystem.actions.FindAction("Thrust");
        fire = InputSystem.actions.FindAction("Fire");
        look = InputSystem.actions.FindAction("Look");
        pause = InputSystem.actions.FindAction("Pause");

        fire.performed += (context) => {
            Fire();
        };
        currentWeapon = gameObject.AddComponent<TestWeapon>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        screenBounds = new Bounds(
            Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)),
            Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)) * 2
        );
    }

    // Update is called once per frame
    void Update()
    {
        steerValue = Mathf.Lerp(steerValue, steer.ReadValue<float>(), 0.5f);
        thrusting = thrust.IsPressed();

        level.transform.Rotate(0, 0, steerValue * steerSpeed * Time.deltaTime);

        if (pause.IsPressed())
        {
            if (!pausing) Pause();
            else Resume();
        }

        lookValue = Vector2.Lerp(lookValue, look.ReadValue<Vector2>(), 0.5f);
        aimDelta = new Vector3(lookValue.x, lookValue.y, 0) * lookSensitivity * Time.deltaTime;
        aim.position = new Vector3(
            Mathf.Clamp(aim.position.x + aimDelta.x, screenBounds.min.x, screenBounds.max.x),
            Mathf.Clamp(aim.position.y + aimDelta.y, screenBounds.min.y, screenBounds.max.y),
            0
        );
    }

    void FixedUpdate()
    {
        if (thrusting)
        {
            rb.AddForceY(thrustPower);
        }
    }

    void Fire()
    {
        currentWeapon.Shoot();
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

    // delete this test code
    class TestWeapon : MonoBehaviour, IWeapon
    {
        GameObject bulletPrefab;
        Bullet bullet;
        Transform aim;

        void Start()
        {
            bulletPrefab = Resources.Load("TestBullet") as GameObject;
            aim = GameObject.Find("Aim").transform;
        }

        public void Shoot()
        {
            bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.direction = (aim.position - transform.position).normalized;
            bullet.speed = 5;
            Debug.Log("Pew pew");
        }
    }
}
