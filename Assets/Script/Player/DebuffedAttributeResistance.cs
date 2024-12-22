using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffedAttributeResistance : MonoBehaviour
{
    private float durationTimer = 0;                //寿命タイマー
    private EnemyHP enemyHpScript;                  //エネミースクリプト

    private float debuffedAttributeResistance;      //属性耐性ダウン値
    private bool debuffedIsTypeMoment;              //耐性ダウンのタイプ
    private float debuffedDuration;                 //持続時間
    private int debuffedCharId;                     //ダウンさせた味方のID
    private int debuffedId;                         //ダウンさせた味方のなかのデバフの種類のID

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //自身の消滅
        durationTimer += Time.deltaTime;
        if (debuffedDuration <= durationTimer)
        {
            Destroy(this.gameObject);
        }
    }

    //デバフの情報を受け取る
    //耐性ダウン値(%)、持続時間がデバフ(時間依存を一瞬で掛けるtrue)かデバフ範囲か(出たら消えるflase)、持続時間、キャラID、攻撃ID
    public void Debuffed(float AttributeResistance, bool isTypeMoment, float duration, int charId, int id) 
    {
        //変数渡し
        debuffedAttributeResistance = AttributeResistance;
        debuffedIsTypeMoment = isTypeMoment;
        debuffedDuration = duration;
        debuffedCharId = charId;
        debuffedId = id;
    }

    //敵との接触
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("eeeeeeeeeeeeeeeee");
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("uuuuuuuuuuuuuuu");
            enemyHpScript = other.gameObject.GetComponent<EnemyHP>();
            //ダメージを与える処理
            if (enemyHpScript != null)
            {
                //デバフ
                enemyHpScript.DebuffedAttributeResistance(debuffedAttributeResistance, debuffedIsTypeMoment, debuffedDuration, debuffedCharId, debuffedId);
            }
        }
    }
}
