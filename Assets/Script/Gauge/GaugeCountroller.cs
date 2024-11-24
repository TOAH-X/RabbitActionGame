using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GaugeCountroller : MonoBehaviour
{
    [SerializeField] float gaugeLength;                              //ゲージの長さ(基本的に数値は1)
    [SerializeField] int maxValue;                                  //値の最大値(最大HPなど)
    [SerializeField] int currentValue;                              //現在の値(現在HPなど)

    [SerializeField] Transform canvasTransform;                     //Canvasを見つける(ダメージ表示用など)
    [SerializeField] GameObject gaugeValueNotationObj;              //ゲージ表記オブジェクト
    [SerializeField] GameObject gaugeValueNotationObjs;             //プレハブのゲージ表記オブジェクト
    [SerializeField] GaugeValueNotationCountroller gaugeValueNotationObjsScript;    //ゲージの表記オブジェクトのスクリプト

    [SerializeField] string thisObjName;                            //アタッチされたオブジェクトの名前(ゲージの種類を管理するため)

    [SerializeField] Player player;                                 //getset用(Playerステータス)
    [SerializeField] EnemyHP enemyHp;                               //getset用(EnemyHP)
    [SerializeField] TeamCoutnroller teamCoutnroller;               //getset用(チームキャラのステータス)

    private RectTransform rectTransform;                            //Imageの情報取得

    // Start is called before the first frame update
    void Start()
    {
        /*
        this.player = FindObjectOfType<Player>(); // インスタンス化
        int currentHp = player.CurrentHp; // ゲッター。ScriptAの変数を取得する
        */
        //getset用にインスタンス化
        this.player = FindObjectOfType<Player>();
        this.enemyHp = FindObjectOfType<EnemyHP>();


        //自身のオブジェクト名を取得
        thisObjName = this.gameObject.name;

        //初期値(仮)
        maxValue = 100;
        currentValue = 100;

        //ゲージの長さの取得
        //gageLength = this.transform.localScale.x;
        rectTransform = GetComponent<RectTransform>();
        gaugeLength = rectTransform.localScale.x;

        //HPゲージのとき限定の処理(数値の表示など)
        if (thisObjName == "HPGaugeMain")
        {
            //Canvasを見つける(HP値表示用など)
            canvasTransform = GameObject.Find("Canvas").transform;

            //HP(ゲージの数値)表記呼び出し(プレハブ)
            gaugeValueNotationObjs = Instantiate<GameObject>(gaugeValueNotationObj, transform.position, Quaternion.identity, canvasTransform);
            gaugeValueNotationObjsScript = gaugeValueNotationObjs.GetComponent<GaugeValueNotationCountroller>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Human human = new Human();
        //human.Height = 150; // Heightで設定
        //Debug.Log(human.Height); // Heightで取得

        //値の取得
        if (thisObjName == "HPGaugeMain")
        {
            //HP値の受け渡し
            maxValue = player.MaxHp;
            currentValue = player.CurrentHp;
        }
        else if (thisObjName == "MPGaugeMain")
        {
            //MP値の受け渡し
            maxValue = player.MaxMp;
            currentValue = player.CurrentMp;
        }
        else if (thisObjName == "StaminaGaugeMain")
        {
            //Stamina値の受け渡し
            maxValue = player.MaxStamina;
            currentValue = player.CurrentStamina;
            //プレイヤーに追従
             
        }
        else if (thisObjName == "EnemyHPGaugeMain") 
        {
            //EnemyHP値の受け渡し
            maxValue = enemyHp.EnemyMaxHp;
            currentValue = enemyHp.EnemyCurrentHp;
        }
        else if (thisObjName == "Sub1HPGaugeMain")
        {
            //Sub1HP値の受け渡し
            maxValue = teamCoutnroller.TeamMaxHp[0];
            currentValue = teamCoutnroller.TeamCurrentHp[0];
        }
        else if (thisObjName == "Sub2HPGaugeMain")
        {
            //Sub2HP値の受け渡し
            maxValue = teamCoutnroller.TeamMaxHp[1];
            currentValue = teamCoutnroller.TeamCurrentHp[1];
        }
        else if (thisObjName == "Sub3HPGaugeMain")
        {
            //Sub3HP値の受け渡し
            maxValue = teamCoutnroller.TeamMaxHp[2];
            currentValue = teamCoutnroller.TeamCurrentHp[2];
        }


        //現在地が最大値を超えた際にはみ出さないようにする処理
        if ((float)((float)currentValue / (float)maxValue) > 1) 
        {
            currentValue = maxValue;
        }

        if (maxValue != 0) 
        {
            //ゲージ長の反映
            rectTransform.localScale = new Vector3(gaugeLength * currentValue / maxValue, 1, 1);
            rectTransform.localPosition = new Vector3(((gaugeLength * currentValue / maxValue) * 0.5f - 0.5f) * rectTransform.sizeDelta.x, 0, 0);
            //this.transform.localScale = new Vector3(gageLength * currentValue / maxValue, 1, 1);
            //this.transform.localPosition = new Vector3((gageLength * currentValue / maxValue) * 0.5f -0.5f, 0, 0);
        }


        //HP(ゲージの値)の更新
        if (thisObjName == "HPGaugeMain") 
        {
            //値表記の反映
            gaugeValueNotationObjsScript.GaugeValueNotation(maxValue, currentValue);
        }
    }
}
