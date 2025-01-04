using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Buffers.Text;
using System;
using System.Reflection;

public class DataManager : MonoBehaviour
{
    public static DataManager Instans { get; private set; }         //データマネージャースクリプト(シングルトン用)

    public List<CharData> charDataList = new List<CharData>();      // キャラクターデータのリスト

    //キャラクターデータベース
    public DB_CharData dB_charData;

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

    // Start is called before the first frame update
    void Start()
    {
        foreach (var element in dB_charData.charData) 
        {
            //データの初期化(最初のみ)
            UpdateJSONData(element.charId, CharDbReferenceCurrentHp(element.charId), 99);      //レベルは仮置き
        }
    }

    //CurretHPの取得(JSON用のこのスクリプトからではなく間に挟む保存用データからアクセスできるようにしておくこと)
    public int GetCurrentHp(int charId)
    {
        CharData charData = charDataList.Find(c => c.id == charId);
        if (charData.id != 0) 
        {
            return charData.currentHp;
        }
        else 
        {
            Debug.Log("エラー:DataManagerで保存していないキャラのCurrentHPが参照されている");
            return -1;
        }
    }

    //キャラ情報をデータベースから参照
    public int CharDbReferenceCurrentHp(int charId)
    {
        //attribute = dB_charData.charData[charId].attribute;                 //属性
        //maxSkillRecharge = dB_charData.charData[charId].maxSkillRecharge;   //スキルクールタイム
        //maxSpecialMoveRecharge = dB_charData.charData[charId].maxSpecialMoveRecharge;   //必殺技クールタイム
        int currentHp = dB_charData.charData[charId].baseHp;                    //基礎HP
        return currentHp;
    }

    //JSONデータ更新用
    public void UpdateJSONData(int id, int currentHp, int level)
    {
        //既にIDが存在するかチェック
        CharData charDataListNum = charDataList.Find(c => c.id == id);

        //既存キャラかどうか
        if (charDataListNum.id != 0)
        {
            //既存キャラ(更新)
            charDataList[charDataListNum.id] = new CharData { id = id, currentHp = currentHp, level = level };
        }
        else
        {
            //新規キャラ(追加)
            charDataList.Add(new CharData { id = id, currentHp = currentHp, level = level });
        }

        //JSONに保存
        SaveJSONData();
    }

    //JSONデータの保存
    public void SaveJSONData()
    {
        string jsonData = JsonUtility.ToJson(new CharDataList { charDataList = charDataList }, true);           //重くなるので通信をするならtrueはflaseにすること。
        File.WriteAllText("Assets/Resources/charData.json", jsonData);
        Debug.Log(JsonUtility.ToJson(jsonData, true));
        Debug.Log("jsonData contents: " + jsonData);
    }

    //JSONデータの呼び出し
    public void LoadJSONData() 
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

//dataManagerScript.UpdateJSONData(1, currentHp, 99);
//dataManagerScript.LoadJSONData();
//dataManagerScript.GetCurrentHp(charId);