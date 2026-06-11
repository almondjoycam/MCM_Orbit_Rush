using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerControls : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    Level level;
    [SerializeField]
    GameObject gameOverScreen;
    [SerializeField]
    TextMeshProUGUI fuelMeter;

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
    public int maxHealth = 4;
    int health;

    // max fuel is discrete (number of tanks)
    // but current fuel is continuous (burning fuel from tanks)
    public int maxFuel = 3;
    float fuel;
    public float fuelDrainPerSecond = 0.1f;
    float fuelTimeCounter = 0;

    public bool shieldActive { get; private set; }

    [System.Serializable]
    public struct PlayerSaveData
    {
        public int health;
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
        gameOverScreen.SetActive(false);

        gameplay = InputSystem.actions.FindActionMap("Player");
        uiCancel = InputSystem.actions.FindAction("UI/Cancel");

        steer = gameplay.FindAction("Steer");
        thrust = gameplay.FindAction("Thrust");
        fire = gameplay.FindAction("Fire");
        look = gameplay.FindAction("Look");
        pause = gameplay.FindAction("Pause");

        pause.performed += (context) => { Pause(); };
        uiCancel.performed += (context) => { Resume(); };
        fire.performed += (context) => { Fire(); };
        currentWeapon = gameObject.AddComponent<TestWeapon>();

        screenBounds = new Bounds(
            Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)),
            Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)) * 2
        );
        ToggleUICursor(false);
    }

    // Update is called once per frame
    void Update()
    {
        steerValue = Mathf.Lerp(steerValue, steer.ReadValue<float>(), 0.5f);
        thrusting = thrust.IsPressed();
        if (fuel <= 0)
        {
            GameOver();
        }
        fuelMeter.text = $"Fuel: {fuel:F1}/{maxFuel}";

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
        Start();
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
