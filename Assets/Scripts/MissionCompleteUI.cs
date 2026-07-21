using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionCompleteUI : MonoBehaviour
{
    public void PlayAgain()
    {
        // Restore normal game speed before reloading.
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        // Only use this if you have a scene named MainMenu.
        SceneManager.LoadScene("MainMenu");
    }


    public void QuitGame()
    {
        Time.timeScale = 1f;

        Debug.Log("Quit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

