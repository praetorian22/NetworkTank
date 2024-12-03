using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad : GenericSingletonClass<SaveLoad>
{
    public SaveData LoadData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath
        + "/SaveData.dat", FileMode.Open);
        SaveData data_load = (SaveData)bf.Deserialize(file);
        file.Close();
        return data_load;
    }
    public void SaveData(SaveData data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/SaveData.dat");
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }
}
[Serializable]
public class SaveData
{
    public string namePlayer;
}