using UnityEngine;

public class LevelUnlocker : MonoBehaviour
{
    private int highestLevelUnlocked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int i = 0;
        highestLevelUnlocked = PlayerPrefs.GetInt("MaxLevel");
        for (; i <= highestLevelUnlocked; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        for (; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
