using UnityEngine;

public class WarpGate : MonoBehaviour
{
    [SerializeField] private GameObject missionCompleteScreen;

    private bool completed;

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

        Debug.Log("Neptune Mission Complete!");

        missionCompleteScreen.SetActive(true);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}