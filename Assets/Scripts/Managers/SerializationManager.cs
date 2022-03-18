using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationManager : MonoBehaviour
{
    private static SaveData _current;
    public static SaveData Current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
            }

            return _current;
        }
    }

    public void Save(string saveName, object saveData)
    {
        var formatter = GetBinaryFormatter();
        string path = Application.persistentDataPath + "/saves";
        var file = File.Create($"{path}/{saveName}.save");
        formatter.Serialize(file, saveData);
        file.Close();
    }

    public object Load(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        var formatter = GetBinaryFormatter();
        var file = File.Open(path, FileMode.Open);

        try
        {
            var save = formatter.Deserialize(file);
            file.Close();
            return save;
        }
        catch (Exception)
        {
            Debug.LogErrorFormat("Failed to load file at {0}", path);
            file.Close();
            return null;
        }
    }

    private BinaryFormatter GetBinaryFormatter()
    {
        var formatter = new BinaryFormatter();
        return formatter;
    }
}
