using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Persistance
{
    public class JsonDataContext : DataContext
    {
        public string FileName;

        public override void Load()
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            var formatter = GetBinaryFormatter();
            var file = File.Open(filePath, FileMode.Open);

            try
            {
                var save = formatter.Deserialize(file);
                file.Close();
                data = (GameData)save;
            }
            catch (Exception)
            {
                Debug.LogErrorFormat("Failed to load file at {0}", filePath);
                file.Close();
            }
        }

        public override void Save()
        {
            var formatter = GetBinaryFormatter();
            var file = File.Create(filePath);
            formatter.Serialize(file, data);
            file.Close();
        }

        private BinaryFormatter GetBinaryFormatter()
        {
            var formatter = new BinaryFormatter();
            return formatter;
        }

        private string filePath => $@"{Application.persistentDataPath}\{FileName}.save";
    }
}
