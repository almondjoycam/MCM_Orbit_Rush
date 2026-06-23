using System.IO;
using UnityEngine;

public class SaveSystem
{
    public static bool Save(string path, string jsonData)
    {
        try
        {
            File.WriteAllText(path, jsonData);
            return true;
        }
        catch (System.SystemException fileError)
        {
            return false;
        }
    }

    public static PlayerControls.PlayerSaveData Load(string path)
    {
        return JsonUtility.FromJson<PlayerControls.PlayerSaveData>(File.ReadAllText(path));
    }
}
