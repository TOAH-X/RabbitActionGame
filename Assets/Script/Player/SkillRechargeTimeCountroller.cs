using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillRechargeTimeCountroller : MonoBehaviour
{
    [SerializeField] Player playerScript;                           //プレイヤースクリプト
    private float currentSkillRecharge = 0;                         //スキルクールタイムの残り時間
    [SerializeField] Text thisText;                                 //テキスト
    private string thisObjName;                                     //オブジェクトの名前(必殺技かスキルか区別するため)

    // Start is called before the first frame update
    void Start()
    {
        thisText = this.GetComponent<Text>();

        //必殺技とスキルのクールタイムを併用する
        thisObjName = this.gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {            
        //クールタイムの取得
        if (thisObjName == "SkillRechargeTime")
        {
            currentSkillRecharge = playerScript.CurrentSkillRecharge;
        }
        else if (thisObjName == "SpecialMoveRechargeTime") 
        {
            currentSkillRecharge = playerScript.CurrentSpecialMoveRecharge;
        }

        if (currentSkillRecharge != 0)
        {
            //クールタイムの表示
            thisText.text = "" + currentSkillRecharge.ToString("n2");
        }
        else 
        {
            if (thisObjName == "SkillRechargeTime")
            {
                //非表示
                thisText.text = "E";
            }
            else if (thisObjName == "SpecialMoveRechargeTime")
            {
                //非表示
                thisText.text = "Q";
            }
                
        }
            
    }
}
