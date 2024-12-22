using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffedAttributeResistance : MonoBehaviour
{
    private float durationTimer = 0;                //�����^�C�}�[
    private EnemyHP enemyHpScript;                  //�G�l�~�[�X�N���v�g

    private float debuffedAttributeResistance;      //�����ϐ��_�E���l
    private bool debuffedIsTypeMoment;              //�ϐ��_�E���̃^�C�v
    private float debuffedDuration;                 //��������
    private int debuffedCharId;                     //�_�E��������������ID
    private int debuffedId;                         //�_�E�������������̂Ȃ��̃f�o�t�̎�ނ�ID

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���g�̏���
        durationTimer += Time.deltaTime;
        if (debuffedDuration <= durationTimer)
        {
            Destroy(this.gameObject);
        }
    }

    //�f�o�t�̏����󂯎��
    //�ϐ��_�E���l(%)�A�������Ԃ��f�o�t(���Ԉˑ�����u�Ŋ|����true)���f�o�t�͈͂�(�o���������flase)�A�������ԁA�L����ID�A�U��ID
    public void Debuffed(float AttributeResistance, bool isTypeMoment, float duration, int charId, int id) 
    {
        //�ϐ��n��
        debuffedAttributeResistance = AttributeResistance;
        debuffedIsTypeMoment = isTypeMoment;
        debuffedDuration = duration;
        debuffedCharId = charId;
        debuffedId = id;
    }

    //�G�Ƃ̐ڐG
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("eeeeeeeeeeeeeeeee");
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("uuuuuuuuuuuuuuu");
            enemyHpScript = other.gameObject.GetComponent<EnemyHP>();
            //�_���[�W��^���鏈��
            if (enemyHpScript != null)
            {
                //�f�o�t
                enemyHpScript.DebuffedAttributeResistance(debuffedAttributeResistance, debuffedIsTypeMoment, debuffedDuration, debuffedCharId, debuffedId);
            }
        }
    }
}
