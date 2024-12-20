using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VacuumRange : MonoBehaviour
{
    private float vacuumDuration;                       //吸引効果時間
    private float durationTimer = 0;                    //吸引時間用タイマー

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //自身の消滅
        durationTimer += Time.deltaTime;
        if (vacuumDuration <= durationTimer) 
        {
            Destroy(this.gameObject);
        }
    }

    //通常攻撃
    public void Vacuum(float duration)
    {
        vacuumDuration = duration;
    }

    //敵との接触
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHP enemyHpScript = other.gameObject.GetComponent<EnemyHP>();
            //吸引効果を与える処理
            if (enemyHpScript != null)
            {
                //吸引効果の座標
                enemyHpScript.EnemyVacuum(this.transform.position.x);
            }
        }
    }
}
