using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ChangeTeamController : MonoBehaviour
{
    [SerializeField] GameObject contentObj;         //contentオブジェクト
    [SerializeField] GameObject charIconObj;        //charIcon(BackGround)オブジェクト

    private Sprite charIcon;                        //キャラアイコン
    private Sprite charFullBodyImage;               //キャラの立ち絵
    private int attribute;                          //キャラの属性

    private int[] teamMemvers = new int[3];         //チームメンバーの変数(メンバー変数でもあるよ（笑）)

    //キャラクターデータベース
    public DB_CharData dB_charData;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var element in dB_charData.charData)  
        {
            //0はデモキャラ用なので除外
            if (element.charId != 0)  
            {
                CharDbReference(element.charId);
                var charIconObjs = Instantiate(charIconObj, this.transform.position, this.transform.rotation);
                charIconObjs.transform.parent = contentObj.transform;
                charIconObjs.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                GameObject childCharIconObj = charIconObjs.transform.Find("CharIcon").gameObject;
                Image childCharIconImage = childCharIconObj.GetComponent<Image>();
                childCharIconImage.sprite = charIcon;
                ChangeTeamIcon charIconObjsScript = charIconObjs.GetComponent<ChangeTeamIcon>();
                charIconObjsScript.SetData(element.charId);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //キャラ情報をデータベースから参照
    public void CharDbReference(int charId)
    {
        charIcon = dB_charData.charData[charId].charIcon;                   //キャラアイコン
        charFullBodyImage = dB_charData.charData[charId].charFullBodyImage; //キャラ立ち絵
        attribute = dB_charData.charData[charId].attribute;                 //属性
    }

    //キャラ情報をデータベースから参照
    public Sprite CharDbReferenceCharFullBodyImage(int charId)
    {
        charFullBodyImage = dB_charData.charData[charId].charFullBodyImage; //キャラ立ち絵
        return charFullBodyImage;
    }
}
