using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    PlayerControls player;
    string savePath;

    void Start()
    {
        savePath = Application.persistentDataPath + "/save.json";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // get player current state
            player = other.gameObject.GetComponent<PlayerControls>();
            string saveState = player.GetSaveState();
            // tell the save system to save to file
            bool saved = SaveSystem.Save(savePath, saveState);
            Debug.Log(saveState + saved);
            // also pull up the checkpoint menu
        }
    }
}
