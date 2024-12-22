using System.Buffers.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] int enemyId = 1;                           //敵のID
    [SerializeField] int enemyMaxHp = 100000;                   //敵の最大HP
    [SerializeField] int enemyCurrentHp = 0;                    //敵の現在HP
    [SerializeField] int enemyAttribute = 0;                    //敵の属性

    //ダメージ表記用
    [SerializeField] GameObject damageNotationObj;              //ダメージ表記オブジェクト
    [SerializeField] Transform canvasTransform;                 //Canvasを見つける(ダメージ表示用など)

    //死亡時用
    [SerializeField] EnemyAction enemyActionScript;             //エネミーアクションスクリプト(死亡時用)
    [SerializeField] int latestAttackCharId = 0;                //最後に自身を攻撃したキャラのID(死亡時用)

    private GameObject playerObj;                               //プレイヤーObj(対消滅ダメージ用)※対消滅ダメージはプレイヤー側(ダメージを与える方)で行う

    [SerializeField] float knockBackGauge = 0;                  //ノックバックゲージ(これがマックスになると吹っ飛ぶ)
    [SerializeField] float KnockBackLimit = 500;                //ノックバックリミット(この値を超えるとノックバックする)※中断体制で値を変更したい、DB参照
    [SerializeField] float knockBackCoolTime = 60;              //ノックバックのクールタイム(deltaTimeに変えましょう)
    private float currentKnockBackCoolTime = 0;                 //次にノックバックをするまでの時間

    private Enemy enemyScript;                                  //Enemyスクリプト
    private EnemyHP enemyHpScript;                              //EnemyHPスクリプト
    private Player playerScript;                                //Playerスクリプト

    [SerializeField] float debuffedAttributeResistance = 0;     //属性耐性

    // Start is called before the first frame update
    void Start()
    {
        enemyHpScript = GetComponent<EnemyHP>();

        //ステータス参照
        enemyScript = this.gameObject.GetComponent<Enemy>();
        enemyAttribute = enemyScript.EnemyAttribute;
        enemyMaxHp = enemyScript.EnemyMaxHp;

        //ステータス代入(HP)
        enemyCurrentHp = enemyMaxHp;

        //Canvasを見つける(ダメージ表示用など)
        canvasTransform = GameObject.Find("Canvas").transform;
        //プレイヤーを見つける(余力があれば軽い処理に書き換えること)
        playerObj = GameObject.Find("Player");
        playerScript = playerObj.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //自身のHPがマイナスになったとき
        if (enemyCurrentHp < 0)
        {
            enemyCurrentHp = 0;
        }
        //死亡処理
        if (enemyCurrentHp == 0)
        {
            enemyActionScript.EnemyDeath(latestAttackCharId);
        }

        //ノックバック値の減少
        if (knockBackGauge > 0) 
        {
            knockBackGauge--;
        }
        //ノックバックのクールタイム
        if (currentKnockBackCoolTime > 0) 
        {
            currentKnockBackCoolTime--;
        }
    }

    //被ダメージ処理
    public void EnemyDamage(int attackCharId, float attackRangePos, int damage, int attribute, bool isAttentionDamage, float knockBackValue, bool isFollowUpAttack)
    {
        //倍率(書き換えること)
        float attributeResistance = (float)((float)GameSystemUtility.CalcDamage(damage, enemyAttribute, attribute, () => StartCoroutine(PairAnnihilationDamage(damage))) /(float) damage);
        //下の処理と代替可能
        damage = GameSystemUtility.CalcDamage(damage, enemyAttribute, attribute, () => StartCoroutine(PairAnnihilationDamage(damage)));
        //属性耐性の倍率から計算
        damage = Mathf.CeilToInt(damage * (attributeResistance + Mathf.Sqrt(debuffedAttributeResistance * (Mathf.Sqrt(1 / attributeResistance)))));

        //属性倍率判定、仮置き(敵味方共通のスクリプトを作ること)
        //damage = AttributeCalculator(damage, attribute);
        enemyCurrentHp -= damage;

        //ダメージ表記呼び出し
        var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
        DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
        damageNotationObjsScript.DamageNotion(damage, attribute, isAttentionDamage, (Vector2)transform.position + new Vector2(0, 0.0f));

        //ノックバッククールタイムがあるときはノックバック値が蓄積しない
        if (currentKnockBackCoolTime <= 0) 
        {
            knockBackGauge += knockBackValue;
        }

        //ノックバック発生
        if (knockBackGauge >= KnockBackLimit) 
        {
            bool isKnockBackRight;
            currentKnockBackCoolTime = knockBackCoolTime;
            //右に吹き飛ぶ
            if (attackRangePos <= this.transform.position.x) 
            {
                isKnockBackRight = true;
                enemyActionScript.EnemyKnockBack(isKnockBackRight);
                Debug.Log("右にノックバック");
            }
            //左に吹き飛ぶ
            else
            {
                isKnockBackRight = false;
                enemyActionScript.EnemyKnockBack(isKnockBackRight);
                Debug.Log("左にノックバック");
            }
            knockBackGauge = 0;
        }

        //最後に攻撃したキャラIDの保存
        latestAttackCharId = attackCharId;

        //追撃
        if (isFollowUpAttack == false) 
        {
            if (playerScript == null) 
            {
                playerScript = playerObj.gameObject.GetComponent<Player>();
            }
            if (playerScript != null)
            {
                playerScript.FollowUpAttack(enemyHpScript);
            }
            
        }

        //Debug.Log("キャラID" + attackCharId + "　与えたダメージ" + damage);
    }

    /*
    //属性倍率判定、仮置き(敵味方共通のスクリプトを作ること)
    public int AttributeCalculator(int damage, int attribute) 
    {
        float damageRate = 1;

        //自身が火
        if (enemyAttribute == 1)  
        {
            if (attribute == 4) 
            {
                damageRate = 2;
            }
            if (attribute == 3)
            {
                damageRate = 2;
            }
        }
        //自身が風
        else if (enemyAttribute == 2)
        {
            if (attribute == 1)
            {
                damageRate = 2;
            }
        }
        //自身が水
        else if (enemyAttribute == 3)
        {
            if (attribute == 2)
            {
                damageRate = 2;
            }
            if (attribute == 1)
            {
                damageRate = 2;
            }
        }
        //自身が土
        else if (enemyAttribute == 4)
        {
            if (attribute == 3)
            {
                damageRate = 2;
            }
        }
        //自身がエーテル
        else if (enemyAttribute == 5)
        {
            if (attribute == 1 || attribute == 2 || attribute == 3 || attribute == 4) 
            {
                damageRate = 1.5f;
            }
            if (attribute == 6)
            {
                //対消滅ダメージを発生
                if (playerObj != null) 
                {
                    StartCoroutine(PairAnnihilationDamage(damage));
                }
            }
        }
        //自身が虚空
        else if (enemyAttribute == 6)
        {
            if (attribute == 1 || attribute == 2 || attribute == 3 || attribute == 4)
            {
                damageRate = 0.5f;
            }
            if (attribute == 5)
            {
                damageRate = 2;
                //対消滅ダメージを発生
                if (playerObj != null)
                {
                    StartCoroutine(PairAnnihilationDamage(damage));
                }
            }
        }

        return Mathf.CeilToInt((float)(damage * damageRate));
    }
    */

    //対消滅ダメージ(0.1秒後に発生)
    IEnumerator PairAnnihilationDamage(int damage) 
    {
        for (int i = 0; i < 12; i++)  
        { 
            yield return null;
        }

        Player playerScript = playerObj.GetComponent<Player>();
        playerScript.AttackMaker(damage, 0, 0, 0, this.transform.position, new Vector2(5, 5), 1000, true);

        yield break;
    }

    //吸引用
    public void EnemyVacuum(float attackRangePos) 
    {
        bool isKnockBackRight;
        //右に吹き飛ぶ
        if (attackRangePos >= this.transform.position.x)
        {
            isKnockBackRight = true;
            enemyActionScript.EnemyKnockBack(isKnockBackRight);
            Debug.Log("右に吸われる");
        }
        //左に吹き飛ぶ
        else
        {
            isKnockBackRight = false;
            enemyActionScript.EnemyKnockBack(isKnockBackRight);
            Debug.Log("左に吸われる");
        }
    }

    //属性耐性ダウン
    public void DebuffedAttributeResistance(float attributeResistance, bool isTypeMoment, float duration, int debuffedCharId, int debuffedId)
    {
        debuffedAttributeResistance = attributeResistance;
        Debug.Log(debuffedAttributeResistance+"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
    }

    //enemyMaxHp参照用(getset)
    public int EnemyMaxHp // プロパティ
    {
        get { return enemyMaxHp; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { enemyMaxHp = value; } // 通称セッター。value はセットする側の数字などを反映する
    }
    //enemyCurrentHp参照用(getset)
    public int EnemyCurrentHp // プロパティ
    {
        get { return enemyCurrentHp; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { enemyCurrentHp = value; } // 通称セッター。value はセットする側の数字などを反映する
    }
}
