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
    [SerializeField] Image charFullBodyImage;       //charFullBodyImageのイメージ
    [SerializeField] ChangeTeamTeamMemberController changeTeamTeamMemberControllerScript;   //ChangeTeamTeamMemberControllerスクリプト

    [SerializeField] GameObject[] charIconObjs;     //

    private Sprite charIcon;                        //キャラアイコン
    private Sprite charFullBodySprite;              //キャラの立ち絵
    private int attribute;                          //キャラの属性

    private int[] teamMemvers = new int[3];         //チームメンバーの変数(メンバー変数でもあるよ（笑）)

    //キャラクターデータベース
    public DB_CharData dB_charData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //編成変更画面を開いたとき
    public void OpenChangeTeam() 
    {
        //アイコンのセット
        SetIcon();
        //TeamMemberの初期化
        changeTeamTeamMemberControllerScript.ChangeTeamMember();
        //立ち絵の初期化
        charFullBodyImage.sprite = CharDbReferenceCharFullBodyImage(1);
    }

    //編成変更画面を閉じたとき
    public void QuitChangeTeam() 
    {
        //チーム編成更新
        changeTeamTeamMemberControllerScript.UpdateTeam();
        //アイコンの消去
        DestroyIcon();
    }

    //立ち絵の更新(ChangeTeamIconの文も行うように)//ソートに対応できるように
    public void ChangeCharFullBodyImage(int id) 
    {
        charFullBodyImage.sprite = CharDbReferenceCharFullBodyImage(id);
    }

    //スクリプトビューにアイコンをセット//ソートに対応できるように
    public void SetIcon() 
    {
        //スクリプトビューにアイコンをセット
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

    //アイコンの消去
    public void DestroyIcon() 
    {
        foreach (Transform child in contentObj.transform) 
        {
            if (child.name == "CharIconBackGround(Clone)")
            {
                Destroy(child.gameObject);
            }
        }
    }

    //キャラ情報をデータベースから参照
    public void CharDbReference(int charId)
    {
        charIcon = dB_charData.charData[charId].charIcon;                   //キャラアイコン
        charFullBodySprite = dB_charData.charData[charId].charFullBodyImage; //キャラ立ち絵
        attribute = dB_charData.charData[charId].attribute;                 //属性
    }

    //キャラ情報をデータベースから参照
    public Sprite CharDbReferenceCharFullBodyImage(int charId)
    {
        charFullBodySprite = dB_charData.charData[charId].charFullBodyImage; //キャラ立ち絵
        return charFullBodySprite;
    }

    //キャラ情報をデータベースから参照
    public Sprite CharDbReferenceCharIcon(int charId)
    {
        Sprite charIconSprite = dB_charData.charData[charId].charIcon;      //キャラアイコン
        return charIconSprite;
    }
}
