using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VacuumRange : MonoBehaviour
{
    private float vacuumDuration;                       //吸引効果時間
    private float vacuumPower;                          //吸引力(標準は0.1)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //自身の消滅
        vacuumDuration -= Time.deltaTime;
        if (vacuumDuration <= 0) 
        {
            Destroy(this.gameObject);
        }
    }

    //吸収
    public void Vacuum(float duration, float power)
    {
        vacuumDuration = duration;
        vacuumPower = power;
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
                enemyHpScript.EnemyVacuum((Vector2)this.transform.position, vacuumDuration, vacuumPower);
            }
        }
    }
}
