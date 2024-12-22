using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    private GameObject staminaParentObj;                            //スタミナゲージの親
    private Vector3 staminaParentObjRotation;                       //スタミナゲージの親の回転情報

    private Color32[] hPGaugeColor = new Color32[4];                //HPゲージの色

    private Image thisImage;                                        //自身のImage

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
        thisObjName = gameObject.name;

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
        //HPゲージの場合(軽量化)
        if (thisObjName == "HPGaugeMain" || thisObjName == "Sub1HPGaugeMain" || thisObjName == "Sub2HPGaugeMain" || thisObjName == "Sub3HPGaugeMain") 
        {
            thisImage = gameObject.GetComponent<Image>();
            hPGaugeColor[0] = new Color32(20, 200, 50, 255);
            hPGaugeColor[1] = new Color32(200, 200, 20, 255);
            hPGaugeColor[2] = new Color32(200, 150, 20, 255);
            hPGaugeColor[3] = new Color32(200, 50, 20, 255);
        }
        //スタミナゲージ
        else if (thisObjName == "StaminaGaugeMain")
        {
            //親の取得
            staminaParentObj = transform.parent.gameObject;
            staminaParentObjRotation = staminaParentObj.transform.localEulerAngles;
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
            //スタミナが最大の時は回転して隠す
            if (maxValue <= currentValue) 
            {
                staminaParentObj.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
            else 
            {
                staminaParentObj.transform.localEulerAngles = staminaParentObjRotation;
            }
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
            //HPゲージの色の変更
            ChangeHPGaugeColor((float)((float)currentValue / (float)maxValue));
        }
        else if (thisObjName == "Sub2HPGaugeMain")
        {
            //Sub2HP値の受け渡し
            maxValue = teamCoutnroller.TeamMaxHp[1];
            currentValue = teamCoutnroller.TeamCurrentHp[1];
            //HPゲージの色の変更
            ChangeHPGaugeColor((float)((float)currentValue / (float)maxValue));
        }
        else if (thisObjName == "Sub3HPGaugeMain")
        {
            //Sub3HP値の受け渡し
            maxValue = teamCoutnroller.TeamMaxHp[2];
            currentValue = teamCoutnroller.TeamCurrentHp[2];
            //HPゲージの色の変更
            ChangeHPGaugeColor((float)((float)currentValue / (float)maxValue));
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
            //HPゲージの色の変更
            ChangeHPGaugeColor((float)((float)currentValue / (float)maxValue));
        }
    }

    //HPゲージのカラーの変更(現在HPの割合に応じて)
    public void ChangeHPGaugeColor(float valueRate) 
    {
        if (valueRate >= 0.75f)
        {
            thisImage.color = hPGaugeColor[0];
        }
        else if (valueRate >= 0.50f) 
        {
            thisImage.color = hPGaugeColor[1];
        }
        else if (valueRate >= 0.25f)
        {
            thisImage.color = hPGaugeColor[2];
        }
        else
        {
            thisImage.color = hPGaugeColor[3];
        }
    }
}
