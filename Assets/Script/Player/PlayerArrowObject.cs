using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowObject : MonoBehaviour
{
    private int arrowCharId = 0;                    //ID
    private float arrowDuration = 0;                //寿命
    private int arrowAttack = 0;                    //攻撃力
    private int arrowAttribute = 0;                 //属性
    private float arrowAttentionDamage = 0;         //会心ダメージ
    private float arrowAttentionRate = 0;           //会心率
    private float arrowKnockBackValue = 0;          //ノックバック量
    private float arrowLaunchAngle = 0;             //射出角度

    private Vector2 arrowAttackRangeSize = Vector2.zero;    //攻撃範囲
    
    private int arrowAttackType = 0;                //キャラ内の矢の種類。通常攻撃:0、その他はキャラごと

    private bool isAttentionDamage = false;         //会心ダメージか


    private Rigidbody2D rb2d;                       //rigidbody

    private Player playerScript;                    //プレイヤースクリプト

    private EnemyHP enemyHpScript;                  //エネミースクリプト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //生成された時(情報の受け取りなど)
    //プレイヤースクリプト、キャラID、攻撃のタイプ(通常攻撃:0、その他はキャラごとに割り当て)、キャラ倍率計算後の攻撃力、属性、会心ダメージ、会心率、攻撃発生場所、攻撃範囲の大きさ(x,y)、ノックバック量、発射角度
    public void Arrow(Player player, int charId, int attackType, int attack, int attribute, float attentionDamage, float attentionRate, Vector2 attackRangeSize, float knockBackValue, float launchAngle)
    {
        //重力、追従するか、を書き加えること
        arrowCharId = charId;
        arrowAttack = attack;
        arrowAttribute = attribute;
        arrowKnockBackValue = knockBackValue;
        arrowAttentionDamage = attentionDamage;
        arrowAttentionRate = attentionRate;
        arrowAttackType = attackType;
        arrowLaunchAngle = launchAngle;
        arrowAttackRangeSize = attackRangeSize;

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

    //敵との接触
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyHpScript = other.gameObject.GetComponent<EnemyHP>();
            //ダメージを与える処理
            if (enemyHpScript != null)
            {
                //攻撃
                Attack();

                //消滅
                ArrowDestroy();
            }
        }
    }

    //攻撃
    public void Attack() 
    {
        //単体直接攻撃用
        if (arrowAttackRangeSize.x == 0 && arrowAttackRangeSize.y == 0) 
        {
            playerScript.SingleAttack(enemyHpScript, arrowAttack, arrowAttribute, arrowAttentionDamage, arrowAttentionRate, arrowKnockBackValue, true);
        }
        //範囲攻撃
        else
        {
            Vector2 attackRangePosition = this.transform.position;
            playerScript.AttackMaker(arrowAttack, arrowAttribute, arrowAttentionDamage, arrowAttentionRate, attackRangePosition, arrowAttackRangeSize, arrowKnockBackValue, false);
        }



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
                    transform.localEulerAngles = new Vector3(0, 0, -Mathf.Atan2(direciton.x, direciton.y) / Mathf.PI * 180 - (arrowAttackType - 5) * 12 * (2f - timer)) * 60 * Time.deltaTime;
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, -Mathf.Atan2(direciton.x, direciton.y) / Mathf.PI * 180) * 60 * Time.deltaTime;
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
}
