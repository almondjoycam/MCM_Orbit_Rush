using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerControls : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer rend;

    [SerializeField]
    Level level;
    [SerializeField]
    GameObject gameOverScreen;
    // [SerializeField]
    // TextMeshProUGUI fuelMeter;
    // [SerializeField]
    // TextMeshProUGUI healthMeter;
    HUDManager hud;

    [Header("Input Variables")]
    public float steerSpeed = 1;
    public float thrustPower = 1;
    public float lookSensitivity = 10;

    InputActionMap gameplay;
    InputAction uiCancel;

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

    // TODO: projectiles depend on the weapon, which will have polymorphism
    IWeapon currentWeapon;
    [SerializeField]
    Transform aim;
    Bounds screenBounds;
    Vector3 aimDelta;

    [Header("Player Stats")]
    public int maxHealth = 50;
    float health;

    // max fuel is discrete (number of tanks)
    // but current fuel is continuous (burning fuel from tanks)
    public int maxFuel = 3;
    float fuel;
    public float fuelDrainPerSecond = 0.1f;
    float fuelTimeCounter = 0;

    public bool shieldActive { get; private set; }

    private Coroutine shieldCoroutine;

    [System.Serializable]
    public struct PlayerSaveData
    {
        public float health;
        public int maxHealth;
        public float fuel;
        public int maxFuel;
        public float fuelDrain;
        public bool shieldActive;
        public string currentLevel;

        public PlayerSaveData(PlayerControls player)
        {
            health = player.health;
            maxHealth = player.maxHealth;
            fuel = player.fuel;
            maxFuel = player.maxFuel;
            fuelDrain = player.fuelDrainPerSecond;
            shieldActive = player.shieldActive;
            currentLevel = player.level.levelData.name;
        }
    }

    void Awake()
    {
        health = maxHealth;
        fuel = maxFuel;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();

        hud = FindAnyObjectByType<HUDManager>();

        gameplay = InputSystem.actions.FindActionMap("Player");
        uiCancel = InputSystem.actions.FindAction("UI/Cancel");

        steer = gameplay.FindAction("Steer");
        thrust = gameplay.FindAction("Thrust");
        fire = gameplay.FindAction("Fire");
        look = gameplay.FindAction("Look");
        pause = gameplay.FindAction("Pause");

        thrust.started += (context) => { Thrust(); };
        pause.performed += (context) => { Pause(); };
        uiCancel.performed += (context) => { Resume(); };
        fire.performed += (context) => { Fire(); };
        currentWeapon = gameObject.AddComponent<TestWeapon>();

        screenBounds = new Bounds(
            Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)),
            Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)) * 2
        );
        Debug.Log(screenBounds);
        ResetUI();
    }

    void ResetUI()
    {
        gameOverScreen.SetActive(false);
        hud.SetHealth(health);
        ToggleUICursor(false);
    }

    // Update is called once per frame
    void Update()
    {
        steerValue = Mathf.Lerp(steerValue, steer.ReadValue<float>(), 0.5f);
        if (steerValue >= -0.5f)
        {
            rend.flipX = false;
        }
        else
        {
            rend.flipX = true;
        }

        thrusting = thrust.IsPressed();
        if (fuel <= 0)
        {
            GameOver();
        }
        hud.SetFuel(fuel);

        level.transform.Rotate(0, 0, steerValue * steerSpeed * Time.deltaTime);

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
        if (thrusting && fuel > 0)
        {
            rb.AddForceY(thrustPower);
            fuelTimeCounter += Time.fixedDeltaTime;
            if (fuelTimeCounter >= 1)
            {
                fuel -= fuelDrainPerSecond;
                fuelTimeCounter = 0;
                Debug.Log($"now I only have {fuel:F2} fuel!");
            }
        }

        if (transform.position.y >= screenBounds.max.y - 1)
        {
            rb.AddForceY(-thrustPower);
        }
    }

    void Thrust()
    {
        // actual thrust handled in FixedUpdate but this is for FX
        anim.SetTrigger("Thrust");
        anim.SetBool("Walking", false);
    }

    void Fire()
    {
        currentWeapon.Shoot();
    }

    void Pause()
    {
        pausing = true;
        ToggleUICursor(true);
    }

    void Resume()
    {
        pausing = false;
        ToggleUICursor(false);
    }

    void GameOver()
    {
        gameOverScreen.SetActive(true);
        ToggleUICursor(true);
    }

    void ToggleUICursor(bool on)
    {
        if (on)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameplay.Disable();
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameplay.Enable();
        }
    }

    public void RestoreHealth(float amount)
    {
        // Adds health, but does not let it go above the max health.
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log("Current Health: " + health);
    }

    public void RestoreBoost(float amount)
    {
        // Adds boost energy, but does not let it go above the max boost amount.
        fuel += amount;
        fuel = Mathf.Clamp(fuel, 0, maxFuel);

        Debug.Log("Current Boost Energy: " + fuel);
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
        shieldActive = true;
        Debug.Log("Shield is active.");
        hud.SetShield(5);   // TODO: make this not a random number

        // Waits for the shield time to run out.
        yield return new WaitForSeconds(duration);

        // Turns the shield off after the timer ends.
        shieldActive = false;
        Debug.Log("Shield ended.");
        hud.SetShield(0);
    }

    public void TakeDamage(float damageAmount)
    {
        // If the shield is active, it blocks the hit instead of taking damage.
        if (shieldActive)
        {
            shieldActive = false;
            Debug.Log("Shield blocked the hit!");
            hud.SetShield(0);
            return;
        }

        // Takes health away from the player.
        health -= damageAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
        hud.SetHealth(health);

        Debug.Log("Player took damage. Current Health: " + health);

        // Checks if the player has run out of health.
        if (health <= 0)
        {
            GameOver();
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        anim.SetBool("Walking", true);
    }

    public string GetSaveState()
    {
        PlayerSaveData saveState = new PlayerSaveData(this);
        return JsonUtility.ToJson(saveState);
    }

    public void LoadSaveState(PlayerSaveData state, Transform checkpoint)
    {
        health = state.health;
        maxHealth = state.maxHealth;
        fuel = state.fuel;
        maxFuel = state.maxFuel;
        fuelDrainPerSecond = state.fuelDrain;
        shieldActive = state.shieldActive;
        level.levelData = Resources.Load(state.currentLevel) as LevelData;
        level.transform.rotation = Quaternion.identity;
        ResetUI();
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
