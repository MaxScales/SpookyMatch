using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData
{
    
    public int[] highScores;
    public int[] stars;

}

public class GameData : MonoBehaviour
{

    public static GameData gameData;
    public SaveData saveData;
    // Start is called before the first frame update
    void Awake()
    {
        if(gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        } else
        {
            Destroy(this.gameObject);
        }
        Load();
    }

    

    public void Save()
    {
        //binaRY FORMATTER TO READ BINARY FILES
        BinaryFormatter formatter = new BinaryFormatter();
        //open stream
        FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Create);

        //copy save
        SaveData data=  new SaveData();
        data = saveData;
        //save data to file
        formatter.Serialize(file, data);
        file.Close();
    }


    private void Load() {
        //checked if there is a save game

        if (File.Exists(Application.persistentDataPath + "/player.dat")) {
            //bin formatter
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);

            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
        } else
        {
            saveData = new SaveData();
           
            saveData.stars= new int[50];
            saveData.highScores= new int[50];
        }

    }

    private void OnDisable()
    {
        Save();
    }

   
}
