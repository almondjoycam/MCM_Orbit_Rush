using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    public string nextScene = "Uranus";

    public void LoadLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}