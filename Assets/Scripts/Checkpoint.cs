using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // probably get some info about the level
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // get player current state
        // tell the save system to save to file
        // also pull up the checkpoint menu
        Debug.Log("save the game here");
    }
}
