using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instans { get; private set; }         //�f�[�^�}�l�[�W���[�X�N���v�g(�V���O���g���p)

    public List<CharData> charDataList = new List<CharData>();      // �L�����N�^�[�f�[�^�̃��X�g

    //�L�����e���̃f�[�^
    [System.Serializable]
    public struct CharData
    {
        public int id;                  //����HP
        public int currentHp;           //����HP
        public int level;               //���x��
    }

    //�S�ẴL�����f�[�^
    [System.Serializable]
    public struct CharDataList
    {
        public List<CharData> charDataList;
    }

    //�f�[�^�X�V�p
    public void UpdateData(int id, int currentHp, int level)
    {
        // ����ID�����݂��邩�`�F�b�N
        CharData charDataListNum = charDataList.Find(c => c.id == id);

        //�����L�������ǂ���
        if (charDataListNum.id != 0)
        {
            //�����L����(�X�V)
            charDataListNum.currentHp = currentHp;
            charDataListNum.level = level;
        }
        else
        {
            //�V�K�L����(�ǉ�)
            charDataList.Add(new CharData { id = id, currentHp = currentHp, level = level });
        }

        //JSON�ɕۑ�
        SaveData();
    }

    //�f�[�^�̕ۑ�
    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(new CharDataList { charDataList = charDataList }, true);           //�d���Ȃ�̂ŒʐM������Ȃ�true��flase�ɂ��邱�ƁB
        File.WriteAllText("Assets/Resources/charData.json", jsonData);
        Debug.Log(JsonUtility.ToJson(jsonData, true));
        Debug.Log("jsonData contents: " + jsonData);
    }

    //�f�[�^�̌Ăяo��
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

    //�V���O���g������
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