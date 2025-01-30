using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerSummonObjects : MonoBehaviour
{
    private int summonCharId = 0;                   //ID
    private float summonDuration = 0;               //寿命
    private int summonAttack = 0;                   //攻撃力
    private int summonAttribute = 0;                //属性
    private int summonHp = 0;                       //HP
    private float summonAttentionDamage = 0;        //会心ダメージ
    private float summonAttentionRate = 0;          //会心率
    private float summonKnockBackValue = 0;         //ノックバック量

    private float durationTimer = 0;                //寿命タイマー

    [SerializeField] GameObject playerObj;          //プレイヤーオブジェクト
    [SerializeField] Player playerScript;           //プレイヤーのスクリプト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //自身の消滅
        durationTimer += Time.deltaTime;
        if (summonDuration <= durationTimer)
        {
            Destroy(this.gameObject);
        }
    }

    //召喚された時(情報の受け取りなど)
    //召喚キャラID、寿命(秒)、攻撃力、HP、会心ダメージ、会心率
    //攻撃力やHPはスナップショット
    //攻撃の場合は倍率計算後の攻撃力、属性、会心ダメージ、会心率、攻撃発生場所、攻撃範囲の大きさ(x,y)、ノックバック量
    public void Summon(int charId, float duration, int attack, int attribute, int hp, float attentionDamage, float attentionRate, float knockBackValue)
    {
        //大量生成されないのでFindは気にしなくてよい
        playerObj = GameObject.Find("Player");
        playerScript = playerObj.GetComponent<Player>();

        summonCharId = charId;
        summonDuration = duration;
        summonAttack = attack;
        summonAttribute= attribute;
        summonKnockBackValue = knockBackValue;
        summonHp = hp;
        summonAttentionDamage = attentionDamage;
        summonAttentionRate = attentionRate;

        //キャラID3用の処理
        if (summonCharId == 3) 
        {
            Char3();
        }
        //キャラID3用の処理
        if (summonCharId == 4)
        {
            Char4();
        }
    }

    //攻撃
    //攻撃力、座標、サイズ
    public void Attack(int attack, Vector2 attackRangePosition, Vector2 attackRangeSize)
    {
        playerScript.AttackMaker(attack, summonAttribute, summonAttentionDamage, summonAttentionRate, attackRangePosition, attackRangeSize, summonKnockBackValue, false);
    }

    //回復
    public void Heal(float heal) 
    {
        float healValue = heal;
        playerScript.Heal(false, healValue);
    }

    //以降キャラごとの処理
    //キャラID3のスキル(後で消すこと)
    public void Char3() 
    {
        StartCoroutine(Char3skill());
    }
    //キャラID4のスキル(後で消すこと)
    public void Char4()
    {
        //仮置きで太陽らしい色にしている
        this.GetComponent<SpriteRenderer>().color = new Color32(200, 50, 50, 60);
        StartCoroutine(Char4skill());
    }

    //キャラID3のスキル
    IEnumerator Char3skill() 
    {
        float timer = 0;
        for (int i = 0; i < 20; i++) 
        {
            Attack((int)(summonAttack * 1.35f), this.transform.position, this.transform.localScale * 10);
            Heal((float)summonHp * 0.20f);
            while (timer <= 0.5f) 
            {
                yield return null;
                timer += Time.deltaTime;
            }
            timer = 0;
        }
        yield break;
    }
    //キャラID4のスキル
    IEnumerator Char4skill()
    {
        float timer = 0;
        for (int i = 0; i < 50; i++)
        {
            Attack((int)(summonAttack * 0.35f), this.transform.position, this.transform.localScale * 1);
            while (timer <= 0.2f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            timer = 0;
        }
        yield break;
    }
}
