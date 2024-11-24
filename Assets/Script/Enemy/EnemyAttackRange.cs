using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    [SerializeField] bool isDestroy = false;                    //true�ɂȂ�Ə���
    [SerializeField] int destroyCounter = 0;                    //Destroy���N��������J�E���^�[
    [SerializeField] int enemyAttack = 0;                       //�G�̍U����
    [SerializeField] int enemyAttribute = 1;                    //�G�̑���

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���g�̏���(�o�ꂩ��1�t���[���ŏ�����悤����)
        destroyCounter++;
        if (isDestroy == true && destroyCounter > 2)
        {
            Destroy(this.gameObject);
        }
    }

    //�ʏ�U��
    public void EnemyAttack(int attack,int attribute)
    {
        enemyAttribute = attribute;
        enemyAttack = attack;

        isDestroy = true;
    }

    //�v���C���[�̔���
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.gameObject.GetComponent<Player>();
            //�_���[�W��^���鏈��
            if (playerScript != null)
            {
                playerScript.Damage(enemyAttack, enemyAttribute);
                Debug.Log("NormalAttack" + enemyAttack);
            }
        }
    }
}