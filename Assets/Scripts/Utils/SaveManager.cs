using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/* Save and load data. Suggest to create a wrapper class that load and store the data so data is automatically saved when changes happens.
// Example data in a wrapper class:
// 
//     public static int Credits
//     {
//         get { return SaveManager.Load<int>("Credits"); }
//         set { SaveManager.Save<int>("Credits", value); }
//     }
// 
// Howard Yang '24
*/

namespace Solstice.Core
{
    /// <summary>
    /// An event published when the tempPlayer's save slot is changed. Used to allow the save manager to hold multiple save slots
    /// </summary>
    public class SaveSlotSelectedEvent
    {
        public int slotSelected;
        
        public SaveSlotSelectedEvent(int _slotSelected)
        {
            slotSelected = _slotSelected;
        }
    }

    /// <summary>
    /// Hold data to be saved / loaded.
    /// </summary>
    [System.Serializable]
    public class Data<T>
    {
        public string Key;
        public T Value;

        public Data(string key, T value)
        {
            Key = key;
            Value = value;
        }

        public string GetAsString()
        {
            return Value.ToString();
        }
    }

    /// <summary>
    /// Class that provides two static functions to store and load a key / value pair into the computer. 
    /// </summary>
    public class SaveManager
    {
        // Name of save file
        const string _sysSaveFile = "_Save.bin";

        static int _saveSlot = 0;
        public static int SaveSlot
        {
            get { return _saveSlot; }
            set
            {
                _saveSlot = value;
                EventBus.Publish(new SaveSlotSelectedEvent(value));
            }
        }

        public static void Save<T>(string Key, T Value, string customFileSlot = "default")
        {
            ////Debug.Log("OnPlayerSave Called");

            if (Value == null)
            {
                Debug.LogError($"[{Key}] of type ({typeof(T)}) Attached value cannot be null.");
                return;
            }

            Data<T> data = new Data<T>(Key, Value);

            BinaryFormatter formatter = new BinaryFormatter();
            string path = GetPath(Key, customFileSlot);

            if (!Directory.Exists(GetDirPath(Key, customFileSlot)))
            {
                Directory.CreateDirectory(GetDirPath(Key, customFileSlot));
            }

            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
            //Debug.Log("Save [" + Key + "] completed to: " + path);
        }

        public static T Load<T>(string Key, string customFileSlot = "default")
        {
            string path = GetPath(Key, customFileSlot);
            if (File.Exists(path) == false)
            {
                // Data does not exist yet, save the default value of the primitive type and return it;
                if (default(T) != null) Save<T>(Key, default(T));
                //if (default(T) != null ) Debug.Log("[" + Key + "] Data does not exist yet. Return: " + default(T).ToString() + ". Path = " + path);
                return default(T);
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Data<T> data = formatter.Deserialize(stream) as Data<T>;
            stream.Close();

            //Debug.Log("[" + Key + "] Exists. Return: " + data.Value.ToString() + ". Path = " + path);
            return data.Value;
        }

        public static void Delete(string Key, string customFileSlot = "default")
        {
            string path = GetPath(Key, customFileSlot);
            File.Delete(path);
        }

        public static string GetPath(string Key, string customFileSlot) => Application.persistentDataPath + "/" + customFileSlot + "/" + "PlayerData_" + Key + "_" + SaveSlot.ToString() + _sysSaveFile;
        public static string GetDirPath(string Key, string customFileSlot) => Application.persistentDataPath + "/" + customFileSlot;
    }

}