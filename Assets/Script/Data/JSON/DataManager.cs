using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Buffers.Text;
using System;
using System.Reflection;

public class DataManager : MonoBehaviour
{
    public static DataManager Instans { get; private set; }         //�f�[�^�}�l�[�W���[�X�N���v�g(�V���O���g���p)

    public List<CharData> charDataList = new List<CharData>();      // �L�����N�^�[�f�[�^�̃��X�g

    //�L�����N�^�[�f�[�^�x�[�X
    public DB_CharData dB_charData;

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

    // Start is called before the first frame update
    void Start()
    {
        foreach (var element in dB_charData.charData) 
        {
            //�f�[�^�̏�����(�ŏ��̂�)
            UpdateJSONData(element.charId, CharDbReferenceCurrentHp(element.charId), 99);      //���x���͉��u��
        }
    }

    //CurretHP�̎擾(JSON�p�̂��̃X�N���v�g����ł͂Ȃ��Ԃɋ��ޕۑ��p�f�[�^����A�N�Z�X�ł���悤�ɂ��Ă�������)
    public int GetCurrentHp(int charId)
    {
        CharData charData = charDataList.Find(c => c.id == charId);
        if (charData.id != 0) 
        {
            return charData.currentHp;
        }
        else 
        {
            Debug.Log("�G���[:DataManager�ŕۑ����Ă��Ȃ��L������CurrentHP���Q�Ƃ���Ă���");
            return -1;
        }
    }

    //�L���������f�[�^�x�[�X����Q��
    public int CharDbReferenceCurrentHp(int charId)
    {
        //attribute = dB_charData.charData[charId].attribute;                 //����
        //maxSkillRecharge = dB_charData.charData[charId].maxSkillRecharge;   //�X�L���N�[���^�C��
        //maxSpecialMoveRecharge = dB_charData.charData[charId].maxSpecialMoveRecharge;   //�K�E�Z�N�[���^�C��
        int currentHp = dB_charData.charData[charId].baseHp;                    //��bHP
        return currentHp;
    }

    //JSON�f�[�^�X�V�p
    public void UpdateJSONData(int id, int currentHp, int level)
    {
        //����ID�����݂��邩�`�F�b�N
        CharData charDataListNum = charDataList.Find(c => c.id == id);

        //�����L�������ǂ���
        if (charDataListNum.id != 0)
        {
            //�����L����(�X�V)
            charDataList[charDataListNum.id] = new CharData { id = id, currentHp = currentHp, level = level };
        }
        else
        {
            //�V�K�L����(�ǉ�)
            charDataList.Add(new CharData { id = id, currentHp = currentHp, level = level });
        }

        //JSON�ɕۑ�
        SaveJSONData();
    }

    //JSON�f�[�^�̕ۑ�
    public void SaveJSONData()
    {
        string jsonData = JsonUtility.ToJson(new CharDataList { charDataList = charDataList }, true);           //�d���Ȃ�̂ŒʐM������Ȃ�true��flase�ɂ��邱�ƁB
        File.WriteAllText("Assets/Resources/charData.json", jsonData);
        Debug.Log(JsonUtility.ToJson(jsonData, true));
        Debug.Log("jsonData contents: " + jsonData);
    }

    //JSON�f�[�^�̌Ăяo��
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

//dataManagerScript.UpdateJSONData(1, currentHp, 99);
//dataManagerScript.LoadJSONData();
//dataManagerScript.GetCurrentHp(charId);