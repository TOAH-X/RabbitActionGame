using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VacuumRange : MonoBehaviour
{
    private float vacuumDuration;                       //�z�����ʎ���
    private float vacuumPower;                          //�z����(�W����0.1)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���g�̏���
        vacuumDuration -= Time.deltaTime;
        if (vacuumDuration <= 0) 
        {
            Destroy(this.gameObject);
        }
    }

    //�z��
    public void Vacuum(float duration, float power)
    {
        vacuumDuration = duration;
        vacuumPower = power;
    }

    //�G�Ƃ̐ڐG
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHP enemyHpScript = other.gameObject.GetComponent<EnemyHP>();
            //�z�����ʂ�^���鏈��
            if (enemyHpScript != null)
            {
                //�z�����ʂ̍��W
                enemyHpScript.EnemyVacuum((Vector2)this.transform.position, vacuumDuration, vacuumPower);
            }
        }
    }
}
