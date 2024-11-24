using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    [SerializeField] bool isDestroy = false;                    //trueになると消滅
    [SerializeField] int destroyCounter = 0;                    //Destroyを起動させるカウンター
    [SerializeField] int enemyAttack = 0;                       //敵の攻撃力
    [SerializeField] int enemyAttribute = 1;                    //敵の属性

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
    public void EnemyAttack(int attack,int attribute)
    {
        enemyAttribute = attribute;
        enemyAttack = attack;

        isDestroy = true;
    }

    //プレイヤーの発見
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.gameObject.GetComponent<Player>();
            //ダメージを与える処理
            if (playerScript != null)
            {
                playerScript.Damage(enemyAttack, enemyAttribute);
                Debug.Log("NormalAttack" + enemyAttack);
            }
        }
    }
}
