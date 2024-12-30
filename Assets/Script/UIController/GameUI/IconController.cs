using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    private string thisObjName;                         //自身の名前

    private int currentCharId;                          //現在の表のキャラID(変更前)
    private int[] currentTeamCharId = new int[3];       //現在のチームキャラのId

    [SerializeField] Sprite charIcon;                   //キャラアイコン
    private Image thisImage;                            //自身のImage

    [SerializeField] TeamCoutnroller teamController;    //チームコントローラスクリプト
    public DB_CharData dB_charData;                     //キャラクターデータベース

    [SerializeField] Color32 deathIconColor = new Color32(125, 125, 125, 175);   //死亡時のキャラアイコンのグレースケール化

    // Start is called before the first frame update
    void Start()
    {
        thisImage = gameObject.GetComponent<Image>();

        //自身のオブジェクト名を取得
        thisObjName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (thisObjName == "PlayerIcon")
        {
            if (currentCharId != teamController.CharId)
            {
                CharChange();
            }
        }
        else if (thisObjName == "Sub1Icon") 
        {
            if (currentTeamCharId[0] != teamController.TeamIdData[0]) 
            {
                TeamChange(0);
            }
            DeathCharIcon(0);
        }
        else if (thisObjName == "Sub2Icon")
        {
            if (currentTeamCharId[1] != teamController.TeamIdData[1])
            {
                TeamChange(1);
            }
            DeathCharIcon(1);
        }
        else if (thisObjName == "Sub3Icon")
        {
            if (currentTeamCharId[2] != teamController.TeamIdData[2])
            {
                TeamChange(2);
            }
            DeathCharIcon(2);
        }
    }

    //キャラチェンジ検出時
    public void CharChange()
    {
        CharDbReference(teamController.CharId);                 //現在使用中のキャラの参照
        if (charIcon != null)
        {
            thisImage.sprite = charIcon;
        }
        else 
        {
            thisImage.sprite = null;
        }
        currentCharId = teamController.CharId;
    }

    //チームチェンジ検出時(表のキャラ)
    public void TeamChange(int teamId)
    {
        CharDbReference(teamController.TeamIdData[teamId]);
        if (charIcon != null)
        {
            thisImage.sprite = charIcon;
        }
        else
        {
            thisImage.sprite = null;
        }
    }

    //死亡キャラのアイコンをグレースケール化
    public void DeathCharIcon(int teamId) 
    {
        if (teamController.TeamCurrentHp[teamId] <= 0 && thisImage.color != deathIconColor) 
        {
            thisImage.color = deathIconColor;
        }
        if (teamController.TeamCurrentHp[teamId] > 0 && thisImage.color == deathIconColor)  
        {
            thisImage.color = new Color32(255, 255, 255, 255);
        }
    }
    //キャラ情報参照
    public void CharDbReference(int updateCharId) 
    {
        charIcon = dB_charData.charData[updateCharId].charIcon;                  //キャラアイコン
    }
}
