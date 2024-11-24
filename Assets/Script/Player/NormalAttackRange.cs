using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackRange : MonoBehaviour
{
    [SerializeField] int normalAttack = 10;                     //�U����
    [SerializeField] int attackCharId;                          //�U���҂̃L����ID
    [SerializeField] bool isAttentionDamage = false;            //��S�_���[�W��
    [SerializeField] bool isDestroy = false;                    //���ł��Ă悢��
    [SerializeField] int destroyCounter = 0;                    //���ŃJ�E���^�[
    [SerializeField] int normalAttackAttribute = 0;             //�U������
    [SerializeField] float knockBackValue;

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
    public void NormalAttack(int charId, int attack, int attribute, float attentionDamage, float attentionRate, float charKnockBackValue)
    {
        attackCharId= charId;
        normalAttack = attack;
        normalAttackAttribute = attribute;
        knockBackValue = charKnockBackValue;

        isAttentionDamage = false;

        //��S���̒��I
        float randomPoint = Random.value * 100;
        if (randomPoint <= attentionRate) 
        {
            normalAttack = (int)((float)(normalAttack) * ((100 + attentionDamage) / 100));
            isAttentionDamage = true;
        }

        isDestroy = true;
    }

    //�G�Ƃ̐ڐG
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHP enemyHpScript = other.gameObject.GetComponent<EnemyHP>();
            //�_���[�W��^���鏈��
            if (enemyHpScript != null) 
            {
                //�U�������L������ID�A�_���[�W�����x���W�A�U���́A�����A��S���ǂ���
                enemyHpScript.EnemyDamage(attackCharId, this.transform.position.x, normalAttack, normalAttackAttribute, isAttentionDamage, knockBackValue);
                //Debug.Log("NormalAttack" + normalAttack);
            }
        }
    }
}
