using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VacuumRange : MonoBehaviour
{
    private float vacuumDuration;                       //�z�����ʎ���
    private float durationTimer = 0;                    //�z�����ԗp�^�C�}�[

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���g�̏���
        durationTimer += Time.deltaTime;
        if (vacuumDuration <= durationTimer) 
        {
            Destroy(this.gameObject);
        }
    }

    //�ʏ�U��
    public void Vacuum(float duration)
    {
        vacuumDuration = duration;
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
                enemyHpScript.EnemyVacuum(this.transform.position.x);
            }
        }
    }
}
