using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowObject : MonoBehaviour
{
    private int arrowCharId = 0;                    //ID
    private float arrowDuration = 0;                //寿命
    private int arrowAttack = 0;                    //攻撃力
    private int arrowAttribute = 0;                 //属性
    private int arrowHp = 0;                        //HP
    private float arrowAttentionDamage = 0;         //会心ダメージ
    private float arrowAttentionRate = 0;           //会心率
    private float arrowKnockBackValue = 0;          //ノックバック量

    private bool isAttentionDamage = false;         //会心ダメージか

    private int arrowAttackType = 0;                //キャラ内の矢の種類。通常攻撃:0、その他はキャラごと

    private Rigidbody2D rb2d;                       //rigidbody

    private Player playerScript;                    //プレイヤースクリプト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //生成された時(情報の受け取りなど)
    //プレイヤースクリプト、キャラID、攻撃のタイプ(通常攻撃:0、その他はキャラごとに割り当て)、キャラ倍率計算後の攻撃力、属性、会心ダメージ、会心率、攻撃発生場所、攻撃範囲の大きさ(x,y)、ノックバック量
    public void Arrow(Player player, int charId, int attackType, int attack, int attribute, float attentionDamage, float attentionRate, float knockBackValue)
    {
        //(初速)、向き、重力、追従するか、範囲ダメージかを書き加えること
        arrowCharId = charId;
        arrowAttack = attack;
        arrowAttribute = attribute;
        arrowKnockBackValue = knockBackValue;
        arrowAttentionDamage = attentionDamage;
        arrowAttentionRate = attentionRate;
        arrowAttackType = attackType;

        rb2d = GetComponent<Rigidbody2D>();

        playerScript = player;

        //キャラID4用の処理
        if (arrowCharId == 4 && attackType >= 1)
        {
            Char4();
        }
    }

    //自身の消滅の管理
    public void ArrowDestroy() 
    {
        Destroy(gameObject);
    }

    //以降キャラごとの処理
    //(消すか名前を変えること)
    public void Char4() 
    {
        

        rb2d.gravityScale = 0f;

        

        StartCoroutine(Char4skill());
    }

    //キャラID4のスキル
    IEnumerator Char4skill()
    {
        GameObject enemyObj = GameObject.FindWithTag("Enemy");      //範囲内に対応させること

        float moveSpeed = -0.01f;

        float timer = 0;
        while (timer <= 2.0f)
        {
            if (enemyObj == null)
            {
                enemyObj = GameObject.FindWithTag("Enemy");
            }
            else
            {
                //ターゲットを向く
                float distance = ((Vector2)enemyObj.transform.position - (Vector2)transform.position).sqrMagnitude;
                Vector2 direciton = new Vector2((enemyObj.transform.position.x - transform.position.x), (enemyObj.transform.position.y - transform.position.y));
                if (timer <= 2f)
                {
                    transform.localEulerAngles = new Vector3(0, 0, -Mathf.Atan2(direciton.x, direciton.y) / Mathf.PI * 180 - (arrowAttackType - 5) * 12 * (2f - timer));
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, -Mathf.Atan2(direciton.x, direciton.y) / Mathf.PI * 180);
                }
            }
            //追尾
            transform.position += (transform.up * moveSpeed) * 60 * Time.deltaTime;

            if (moveSpeed <= 5) 
            {
                moveSpeed += 0.003f * 60 * Time.deltaTime;
            }

            /*
            //追尾セット
            //ターゲットを向く
            float distance = ((Vector2)enemyObj.transform.position - (Vector2)transform.position).sqrMagnitude;
            Vector2 direciton = new Vector2((enemyObj.transform.position.x - transform.position.x), (enemyObj.transform.position.y - transform.position.y));
            transform.localEulerAngles = new Vector3(0, 0, -Mathf.Atan2(direciton.x, direciton.y) / Mathf.PI * 180);
            //追尾
            transform.position += (transform.up * moveSpeed);
            */

            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0;
        ArrowDestroy();
        yield break;
    }

    //敵との接触
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHP enemyHpScript = other.gameObject.GetComponent<EnemyHP>();
            //ダメージを与える処理
            if (enemyHpScript != null)
            {

                //範囲攻撃
                //Vector2 attackRangePosition = this.transform.position;
                //Vector2 attackRangeSize = new Vector2(0.5f, 0.5f);
                //playerScript.AttackMaker(arrowAttack, arrowAttribute, arrowAttentionDamage, arrowAttentionRate, attackRangePosition, attackRangeSize, arrowKnockBackValue);

                //単体直接攻撃用
                //会心率の抽選
                float randomPoint = Random.value * 100;
                if (randomPoint <= arrowAttentionRate)
                {
                    arrowAttack = (int)((float)(arrowAttack) * ((100 + arrowAttentionDamage) / 100));
                    isAttentionDamage = true;
                }
                //攻撃したキャラのID、ダメージ判定のx座標、攻撃力、属性、会心かどうか
                enemyHpScript.EnemyDamage(arrowCharId, this.transform.position.x, arrowAttack, arrowAttribute, isAttentionDamage, arrowKnockBackValue);
                

                //仮置き
                ArrowDestroy();
            }
        }
    }
}
