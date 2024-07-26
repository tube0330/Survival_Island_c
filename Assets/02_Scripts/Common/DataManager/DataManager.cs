using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DataInfo;

public class DataManager : MonoBehaviour
{
    [SerializeField] string dataPath;   //저장경로 변수

    public void Initialize()
    {
        dataPath = Application.persistentDataPath + "/gameData.dat";
    }

    public void Save(GameData gameData)
    {
        BinaryFormatter binary = new BinaryFormatter();
        FileStream file = File.Create(dataPath);

        GameData data = new GameData();
        data.equipItem = gameData.equipItem;
        data.killcnt = gameData.killcnt;
        data.HP = gameData.HP;
        data.damage = gameData.damage;
        data.speed = gameData.speed;

        binary.Serialize(file, data);
        file.Close();
    }

    public GameData Load()
    {
        if(File.Exists(dataPath))
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)binary.Deserialize(file);

            file.Close();

            return data;
        }

        else
        {
            GameData data = new GameData();
            
            return data;
        }
    }
}
