using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VacuumRange : MonoBehaviour
{
    private float vacuumDuration;                       //‹zˆøŒø‰ÊŠÔ
    private float durationTimer = 0;                    //‹zˆøŠÔ—pƒ^ƒCƒ}[

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //©g‚ÌÁ–Å
        durationTimer += Time.deltaTime;
        if (vacuumDuration <= durationTimer) 
        {
            Destroy(this.gameObject);
        }
    }

    //’ÊíUŒ‚
    public void Vacuum(float duration)
    {
        vacuumDuration = duration;
    }

    //“G‚Æ‚ÌÚG
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHP enemyHpScript = other.gameObject.GetComponent<EnemyHP>();
            //‹zˆøŒø‰Ê‚ğ—^‚¦‚éˆ—
            if (enemyHpScript != null)
            {
                //‹zˆøŒø‰Ê‚ÌÀ•W
                enemyHpScript.EnemyVacuum(this.transform.position.x);
            }
        }
    }
}
