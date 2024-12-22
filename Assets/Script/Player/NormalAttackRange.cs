using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackRange : MonoBehaviour
{
    private int normalAttack = 10;                              //攻撃力
    private int attackCharId;                                   //攻撃者のキャラID
    private bool isAttentionDamage = false;                     //会心ダメージか
    private bool isDestroy = false;                             //消滅してよいか
    private int destroyCounter = 0;                             //消滅カウンター
    private int normalAttackAttribute = 0;                      //攻撃属性
    private float knockBackValue;                               //ノックバック量
    private bool isFollowUpAttack;                              //追撃かどうか

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //自身の消滅(登場から1フレームで消えるよう調整)
        destroyCounter++;
        if (isDestroy == true && destroyCounter > 2)  
        {
            Destroy(this.gameObject);
        }
    }

    //通常攻撃
    public void NormalAttack(int charId, int attack, int attribute, float attentionDamage, float attentionRate, float charKnockBackValue,bool charIsFollowUpAttack)
    {
        attackCharId= charId;
        normalAttack = attack;
        normalAttackAttribute = attribute;
        knockBackValue = charKnockBackValue;
        isFollowUpAttack = charIsFollowUpAttack;

        isAttentionDamage = false;

        //会心率の抽選
        float randomPoint = Random.value * 100;
        if (randomPoint <= attentionRate) 
        {
            normalAttack = (int)((float)(normalAttack) * ((100 + attentionDamage) / 100));
            isAttentionDamage = true;
        }

        isDestroy = true;
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
                //攻撃したキャラのID、ダメージ判定のx座標、攻撃力、属性、会心かどうか、追撃かどうか
                enemyHpScript.EnemyDamage(attackCharId, this.transform.position.x, normalAttack, normalAttackAttribute, isAttentionDamage, knockBackValue, isFollowUpAttack);
                //Debug.Log("NormalAttack" + normalAttack);
            }
        }
    }
}
