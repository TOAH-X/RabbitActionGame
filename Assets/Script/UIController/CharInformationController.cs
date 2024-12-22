using System.Buffers.Text;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharInformationController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI charNameText;                  //キャラ名テキスト
    [SerializeField] TextMeshProUGUI charLevelText;                 //キャラレベルテキスト
    [SerializeField] TextMeshProUGUI charAttributeText;             //属性テキスト
    [SerializeField] TextMeshProUGUI charWeaponTypeText;            //武器種テキスト
    [SerializeField] TextMeshProUGUI charAttentionRateText;         //会心率表記テキスト
    [SerializeField] TextMeshProUGUI charAttentionDamageText;       //会心ダメージ表記テキスト
    [SerializeField] TextMeshProUGUI charHpText;                    //HP表記テキスト
    [SerializeField] TextMeshProUGUI charAttackText;                //攻撃力表記テキスト
    [SerializeField] TextMeshProUGUI characteristicNameText;        //特性名テキスト
    [SerializeField] TextMeshProUGUI characteristicExplanationText; //特性説明テキスト
    [SerializeField] TextMeshProUGUI skillNameText;                 //スキル名テキスト
    [SerializeField] TextMeshProUGUI skillExplanationText;          //スキル説明テキスト
    [SerializeField] TextMeshProUGUI specialMoveNameText;           //必殺技名テキスト
    [SerializeField] TextMeshProUGUI specialMoveExplanationText;    //必殺技説明テキスト

    [SerializeField] Image charFullBodyImage;                       //立ち絵

    [SerializeField] GameObject charInformationBackGround;          //キャラ情報の背景

    [SerializeField] GameObject playerObj;                          //プレイヤーオブジェクト
    [SerializeField] Player playerScript;                           //プレイヤースクリプト

    private int charId = 0;                                         //参照するキャラID

    private string charName;                                        //キャラ名
    private int attribute;                                          //属性
    private int weaponType;                                         //武器種
    private string characteristicName;                              //特性名
    private string characteristicExplanation;                       //特性説明
    private string skillName;                                       //スキル名
    private string skillExplanation;                                //スキル説明
    private string specialMoveName;                                 //必殺技名
    private string specialMoveExplanation;                          //必殺技説明

    public DB_CharData dB_charData;                                 //キャラクターデータベース

    // Start is called before the first frame update
    void Start()
    {
        playerScript = playerObj.GetComponent<Player>();
        charId = playerScript.CharId;
        CharDbReference();
        charInformationText();
    }

    // Update is called once per frame
    void Update()
    {
        if (charInformationBackGround.activeSelf == false)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                charInformationBackGround.SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                charInformationBackGround.SetActive(false);
            }
        }

        charId = playerScript.CharId;
        CharDbReference();
        charInformationText();
    }

    //キャラ情報をデータベースから参照
    public void CharDbReference()
    {
        charName = dB_charData.charData[charId].charName;                                   //キャラ名
        attribute = dB_charData.charData[charId].attribute;                                 //属性
        weaponType = dB_charData.charData[charId].weaponType;                               //武器種
        characteristicName = dB_charData.charData[charId].characteristicName;               //特性名
        characteristicExplanation = dB_charData.charData[charId].characteristicExplanation; //特性説明
        skillName = dB_charData.charData[charId].skillName;                                 //スキル名
        skillExplanation = dB_charData.charData[charId].skillExplanation;                   //スキル説明
        specialMoveName = dB_charData.charData[charId].specialMoveName;                     //必殺技名
        specialMoveExplanation = dB_charData.charData[charId].specialMoveExplanation;       //必殺技説明
        charFullBodyImage.sprite = dB_charData.charData[charId].charFullBodyImage;          //立ち絵
    }

    //キャラ情報をテキストに反映
    public void charInformationText() 
    {
        string attributeName = "----";
        if (attribute == 0)
        {
            attributeName = "物理(そんな属性はない)";
        }
        else if (attribute == 1) 
        {
            attributeName = "火";
        }
        else if (attribute == 2)
        {
            attributeName = "風";
        }
        else if (attribute == 3)
        {
            attributeName = "水";
        }
        else if (attribute == 4)
        {
            attributeName = "土";
        }
        else if (attribute == 5)
        {
            attributeName = "エーテル";
        }
        else if (attribute == 6)
        {
            attributeName = "虚空";
        }
        else
        {
            attributeName = "そんな属性はない";
        }
        charNameText.text = charName;                                                           //キャラ名
        charLevelText.text = "Lv." + 99;                                                        //キャラレベルテキスト
        charAttributeText.text = "属性:" + attributeName;                                       //属性テキスト
        charWeaponTypeText.text = "武器種:----";                                                  //武器種テキスト
        charAttentionRateText.text = "会心率:" + playerScript.AttentionRate.ToString("F1") + "%";              //会心率表記テキスト
        charAttentionDamageText.text = "会心ダメージ:" + playerScript.AttentionDamage.ToString("F1") + "%";    //会心ダメージ表記テキスト
        charHpText.text = "最大HP:" + playerScript.MaxHp;                                           //HP表記テキスト
        charAttackText.text = "攻撃力:" + playerScript.Attack;                                  //攻撃力表記テキスト
        characteristicNameText.text = "特性「" + characteristicName + "」";                     //特性名テキスト
        characteristicExplanationText.text = characteristicExplanation;                         //特性説明テキスト
        skillNameText.text = "スキル「" + skillName + "」";                                     //スキル名テキスト
        skillExplanationText.text = skillExplanation;                                           //スキル説明テキスト
        specialMoveNameText.text = "必殺技「" + specialMoveName + "」";                         //必殺技名テキスト
        specialMoveExplanationText.text = specialMoveExplanation;                               //必殺技説明テキスト
    }
}
