using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    PlayerControls player;
    string savePath;

    void Start()
    {
        savePath = Application.persistentDataPath + "/save.json";
        player = FindAnyObjectByType<PlayerControls>();
        SaveGame();
    }

    void SaveGame()
    {
        // get player current state
        string saveState = player.GetSaveState();
        // tell the save system to save to file
        bool saved = SaveSystem.Save(savePath, saveState);
        Debug.Log(saveState + saved);
        // also pull up the checkpoint menu
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<PlayerControls>();
            SaveGame();
        }
    }

    public void Retry()
    {
        player.LoadSaveState(SaveSystem.Load(savePath), transform);
    }
}
