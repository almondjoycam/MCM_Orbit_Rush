using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public float gravityConstant;
    public float rotationRate;
    public string levelName;
    public ulong distanceToSun;
    public GameObject levelTerrain;
    public int thresholdItemNumber;
}
