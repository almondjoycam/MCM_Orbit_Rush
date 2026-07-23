using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer rend;

    [SerializeField] private Level level;
    [SerializeField] private GameObject gameOverScreen;
    HUDManager hud;

    [Header("Input Variables")]
    public float steerSpeed = 1f;
    public float thrustPower = 1f;
    public float lookSensitivity = 10f;

    private InputActionMap gameplay;
    private InputAction uiCancel;

    private InputAction steer;
    private InputAction thrust;
    private InputAction fire;
    private InputAction look;
    private InputAction pause;

    private float steerValue;
    private Vector2 lookValue;
    private bool thrusting;
    private bool pausing;

    private IWeapon currentWeapon;

    [SerializeField] private Transform aim;

    private Bounds screenBounds;
    private Vector3 aimDelta;

    [Header("Player Stats")]
    public int maxHealth = 50;
    private float health;

    public int maxFuel = 3;
    private float fuel;

    public float fuelDrainPerSecond = 0.1f;

    private float fuelTimeCounter;

    public bool shieldActive { get; private set; }

    private Coroutine shieldCoroutine;
    private bool gameIsOver;

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

            currentLevel =
                player.level != null && player.level.levelData != null
                    ? player.level.levelData.name
                    : string.Empty;
        }
    }

    private void Awake()
    {
        health = maxHealth;
        fuel = maxFuel;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        hud = FindAnyObjectByType<HUDManager>();
        SetupInputActions();
        SetupWeapon();
        CalculateScreenBounds();
        ResetUI();
    }

    private void SetupInputActions()
    {
        gameplay = InputSystem.actions.FindActionMap("Player");
        uiCancel = InputSystem.actions.FindAction("UI/Cancel");

        if (gameplay == null)
        {
            Debug.LogError(
                "PlayerControls could not find the Player action map."
            );

            enabled = false;
            return;
        }

        steer = gameplay.FindAction("Steer");
        thrust = gameplay.FindAction("Thrust");
        fire = gameplay.FindAction("Fire");
        look = gameplay.FindAction("Look");
        pause = gameplay.FindAction("Pause");

        if (thrust != null)
        {
            thrust.started += OnThrustStarted;
        }

        if (pause != null)
        {
            pause.performed += OnPausePerformed;
        }

        if (uiCancel != null)
        {
            uiCancel.performed += OnCancelPerformed;
        }

        if (fire != null)
        {
            fire.performed += OnFirePerformed;
        }
    }

    private void SetupWeapon()
    {
        currentWeapon = gameObject.AddComponent<TestWeapon>();
    }

    private void CalculateScreenBounds()
    {
        if (Camera.main == null)
        {
            Debug.LogError(
                "PlayerControls could not find a camera tagged MainCamera."
            );

            return;
        }

        Vector3 center =
            Camera.main.ViewportToWorldPoint(
                new Vector3(0.5f, 0.5f, 0f)
            );

        Vector3 topRight =
            Camera.main.ViewportToWorldPoint(
                new Vector3(1f, 1f, 0f)
            );

        Vector3 size = new Vector3(
            Mathf.Abs(topRight.x - center.x) * 2f,
            Mathf.Abs(topRight.y - center.y) * 2f,
            0f
        );

        screenBounds = new Bounds(center, size);
    }

    private void ResetUI()
    {
        gameIsOver = false;

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        hud.SetHealth(health);
        ToggleUICursor(false);
    }

    private void Update()
    {
        if (gameIsOver || gameplay == null)
        {
            return;
        }

        UpdateSteering();
        UpdateThrustInput();
        UpdateLevelRotation();
        UpdateAim();
    }

    private void UpdateSteering()
    {
        if (steer == null)
        {
            return;
        }

        steerValue = Mathf.Lerp(
            steerValue,
            steer.ReadValue<float>(),
            0.5f
        );

        if (rend != null)
        {
            rend.flipX = steerValue < -0.5f;
        }
    }

    private void UpdateThrustInput()
    {
        thrusting = thrust != null && thrust.IsPressed();

        if (fuel <= 0f)
        {
            fuel = 0f;
            GameOver();
        }
        hud.SetFuel(fuel);
    }

    private void UpdateLevelRotation()
    {
        if (level == null)
        {
            return;
        }

        level.transform.Rotate(
            0f,
            0f,
            steerValue * steerSpeed * Time.deltaTime
        );
    }

    private void UpdateAim()
    {
        if (look == null || aim == null)
        {
            return;
        }

        lookValue = Vector2.Lerp(
            lookValue,
            look.ReadValue<Vector2>(),
            0.5f
        );

        aimDelta =
            new Vector3(lookValue.x, lookValue.y, 0f) *
            lookSensitivity *
            Time.deltaTime;

        aim.position = new Vector3(
            Mathf.Clamp(
                aim.position.x + aimDelta.x,
                screenBounds.min.x,
                screenBounds.max.x
            ),
            Mathf.Clamp(
                aim.position.y + aimDelta.y,
                screenBounds.min.y,
                screenBounds.max.y
            ),
            0f
        );
    }

    private void FixedUpdate()
    {
        if (gameIsOver || rb == null)
        {
            return;
        }

        HandleThrust();
        KeepPlayerInsideScreen();
    }

    private void HandleThrust()
    {
        if (!thrusting || fuel <= 0f)
        {
            return;
        }

        rb.AddForceY(thrustPower);

        fuelTimeCounter += Time.fixedDeltaTime;

        if (fuelTimeCounter >= 1f)
        {
            fuel -= fuelDrainPerSecond;
            fuel = Mathf.Clamp(fuel, 0f, maxFuel);

            fuelTimeCounter = 0f;

            // Uncomment while debugging fuel:
            // Debug.Log($"Fuel remaining: {fuel:F2}");
        }
    }

    private void KeepPlayerInsideScreen()
    {
        if (transform.position.y >= screenBounds.max.y - 1f)
        {
            rb.AddForceY(-thrustPower);
        }
    }

    // Input callbacks

    private void OnThrustStarted(
        InputAction.CallbackContext context
    )
    {
        Thrust();
    }

    private void OnPausePerformed(
        InputAction.CallbackContext context
    )
    {
        Pause();
    }

    private void OnCancelPerformed(
        InputAction.CallbackContext context
    )
    {
        Resume();
    }

    private void OnFirePerformed(
        InputAction.CallbackContext context
    )
    {
        Fire();
    }

    private void Thrust()
    {
            // Movement is handled in FixedUpdate.
            // Add engine flame, sound, or particles here later.
    }

    private void Fire()
    {
        if (gameIsOver || currentWeapon == null)
        {
            return;
        }

        currentWeapon.Shoot();
    }

    private void Pause()
    {
        if (gameIsOver)
        {
            return;
        }

        pausing = true;
        ToggleUICursor(true);
    }

    private void Resume()
    {
        if (gameIsOver)
        {
            return;
        }

        pausing = false;
        ToggleUICursor(false);
    }

    private void GameOver()
    {
        if (gameIsOver)
        {
            return;
        }

        gameIsOver = true;
        thrusting = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        ToggleUICursor(true);
    }

    private void ToggleUICursor(bool on)
    {
        if (on)
        {
            Time.timeScale = 0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            gameplay?.Disable();
        }
        else
        {
            Time.timeScale = 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            gameplay?.Enable();
        }
    }

    public void RestoreHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0f, maxHealth);

        Debug.Log("Current Health: " + health);
    }

    public void RestoreBoost(float amount)
    {
        fuel += amount;
        fuel = Mathf.Clamp(fuel, 0f, maxFuel);

        Debug.Log("Current Boost Energy: " + fuel);
    }

    public void ActivateShield(float duration)
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }

        shieldCoroutine =
            StartCoroutine(ShieldTimer(duration));
    }

    private IEnumerator ShieldTimer(float duration)
    {
        shieldActive = true;
        Debug.Log("Shield is active.");
        hud.SetShield(5);   // TODO: make this not a random number

        yield return new WaitForSeconds(duration);

        shieldActive = false;
        shieldCoroutine = null;

        Debug.Log("Shield ended.");
        hud.SetShield(0);
    }

    public void TakeDamage(float damageAmount)
    {
        if (gameIsOver)
        {
            return;
        }

        if (shieldActive)
        {
            shieldActive = false;
            Debug.Log("Shield blocked the hit!");
            hud.SetShield(0);
            return;
        }

        health -= damageAmount;
        health = Mathf.Clamp(health, 0f, maxHealth);
        hud.SetHealth(health);

        Debug.Log(
            "Player took damage. Current Health: " + health
        );

        if (health <= 0f)
        {
            GameOver();
        }
    }

    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (anim != null)
        {
            anim.SetBool("Walking", false);
        }
    }

    public string GetSaveState()
    {
        PlayerSaveData saveState =
            new PlayerSaveData(this);

        return JsonUtility.ToJson(saveState);
    }

    public void LoadSaveState(
        PlayerSaveData state,
        Transform checkpoint
    )
    {
        health = state.health;
        maxHealth = state.maxHealth;

        fuel = state.fuel;
        maxFuel = state.maxFuel;

        fuelDrainPerSecond = state.fuelDrain;
        shieldActive = state.shieldActive;

        if (level != null &&
            !string.IsNullOrEmpty(state.currentLevel))
        {
            level.levelData =
                Resources.Load<LevelData>(state.currentLevel);

            level.transform.rotation = Quaternion.identity;
        }

        if (checkpoint != null)
        {
            transform.position = checkpoint.position;
        }

        ResetUI();
    }

    private void OnDestroy()
    {
        // Remove callbacks belonging to this player before the
        // scene reloads. This prevents destroyed object errors.

        if (thrust != null)
        {
            thrust.started -= OnThrustStarted;
        }

        if (pause != null)
        {
            pause.performed -= OnPausePerformed;
        }

        if (uiCancel != null)
        {
            uiCancel.performed -= OnCancelPerformed;
        }

        if (fire != null)
        {
            fire.performed -= OnFirePerformed;
        }

        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
            shieldCoroutine = null;
        }
    }

    // Delete or replace this test weapon when the final
    // weapon system is ready.
    private class TestWeapon : MonoBehaviour, IWeapon
    {
        private GameObject bulletPrefab;
        private Transform aim;

        private void Start()
        {
            bulletPrefab =
                Resources.Load<GameObject>("TestBullet");

            GameObject aimObject = GameObject.Find("Aim");

            if (aimObject != null)
            {
                aim = aimObject.transform;
            }
            else
            {
                Debug.LogError(
                    "TestWeapon could not find the Aim object."
                );
            }
        }

        public void Shoot()
        {
            if (bulletPrefab == null || aim == null)
            {
                return;
            }

            GameObject bulletObject = Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

            Bullet bullet =
                bulletObject.GetComponent<Bullet>();

            if (bullet == null)
            {
                Debug.LogError(
                    "The TestBullet prefab has no Bullet component."
                );

                Destroy(bulletObject);
                return;
            }

            bullet.direction =
                (aim.position - transform.position).normalized;

            bullet.speed = 5f;

            Debug.Log("Pew pew");
        }
    }
}
