using UnityEngine;

public class WarpGate : MonoBehaviour
{
    [SerializeField] private GameObject missionCompleteScreen;
    private Level level;

    private bool completed;

    void Start()
    {
        level = FindAnyObjectByType<Level>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore asteroids, bullets, pickups, and other objects.
        if (!other.CompareTag("Player"))
            return;

        if (completed)
            return;

        PlayerControls player =
            other.GetComponentInParent<PlayerControls>();

        if (player == null)
        {
            Debug.LogError(
                "Player entered the Warp Gate, but PlayerControls was not found.",
                other
            );
            return;
        }

        if (missionCompleteScreen == null)
        {
            Debug.LogError(
                "Mission Complete Screen is not assigned on WarpGate.",
                this
            );
            return;
        }

        completed = true;

        // Debug.Log("Neptune Mission Complete!");
        PlayerPrefs.SetInt(
            "MaxLevel",
            Mathf.Max(
                level.levelData.levelNumber + 1,
                PlayerPrefs.GetInt("MaxLevel")
                )
            );

        missionCompleteScreen.SetActive(true);

        player.ToggleUICursor(true);
    }
}
