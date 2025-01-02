using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instans { get; private set; }         //データマネージャースクリプト(シングルトン用)

    public List<CharData> charDataList = new List<CharData>();      // キャラクターデータのリスト

    //キャラ各自のデータ
    [System.Serializable]
    public struct CharData
    {
        public int id;                  //現在HP
        public int currentHp;           //現在HP
        public int level;               //レベル
    }

    //全てのキャラデータ
    [System.Serializable]
    public struct CharDataList
    {
        public List<CharData> charDataList;
    }

    //データ更新用
    public void UpdateData(int id, int currentHp, int level)
    {
        // 既にIDが存在するかチェック
        CharData charDataListNum = charDataList.Find(c => c.id == id);

        //既存キャラかどうか
        if (charDataListNum.id != 0)
        {
            //既存キャラ(更新)
            charDataListNum.currentHp = currentHp;
            charDataListNum.level = level;
        }
        else
        {
            //新規キャラ(追加)
            charDataList.Add(new CharData { id = id, currentHp = currentHp, level = level });
        }

        //JSONに保存
        SaveData();
    }

    //データの保存
    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(new CharDataList { charDataList = charDataList }, true);           //重くなるので通信をするならtrueはflaseにすること。
        File.WriteAllText("Assets/Resources/charData.json", jsonData);
        Debug.Log(JsonUtility.ToJson(jsonData, true));
        Debug.Log("jsonData contents: " + jsonData);
    }

    //データの呼び出し
    public void LoadData() 
    {
        string filePath = "Assets/Resources/charaData.json";

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            CharDataList charList = JsonUtility.FromJson<CharDataList>(jsonData);
            charDataList = charList.charDataList;
            Debug.Log(JsonUtility.ToJson(jsonData, true));
        }
    }

    //シングルトン処理
    private void Awake()
    {
        if (Instans != null && Instans != this) 
        {
            Destroy(this.gameObject);
        }
        else 
        { 
            Instans = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}

//dataManagerScript.UpdateData(1, currentHp, 99);
//dataManagerScript.LoadData();