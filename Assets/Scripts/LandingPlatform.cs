using System.Collections;
using UnityEngine;

public class LandingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject missionCompleteScreen;

    private bool completed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (completed)
            return;

        PlayerControls player =
            other.GetComponentInParent<PlayerControls>();

        if (player == null)
            return;

        completed = true;

        StartCoroutine(CompleteLevel());
    }

    IEnumerator CompleteLevel()
    {
        Debug.Log("Mission Complete!");

        // Stop hazards
        Level level = FindAnyObjectByType<Level>();

        if (level != null)
            level.enabled = false;

        // Hide landing platform
        GetComponent<SpriteRenderer>().enabled = false;

        // Let player celebrate
        yield return new WaitForSeconds(2f);

        missionCompleteScreen.SetActive(true);

        Time.timeScale = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
