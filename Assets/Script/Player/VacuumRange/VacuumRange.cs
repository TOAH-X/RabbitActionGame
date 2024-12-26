using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VacuumRange : MonoBehaviour
{
    private float vacuumDuration;                       //‹zˆøŒø‰ÊŠÔ
    private float vacuumPower;                          //‹zˆø—Í(•W€‚Í0.1)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //©g‚ÌÁ–Å
        vacuumDuration -= Time.deltaTime;
        if (vacuumDuration <= 0) 
        {
            Destroy(this.gameObject);
        }
    }

    //‹zû
    public void Vacuum(float duration, float power)
    {
        vacuumDuration = duration;
        vacuumPower = power;
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
                enemyHpScript.EnemyVacuum((Vector2)this.transform.position, vacuumDuration, vacuumPower);
            }
        }
    }
}
