using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerSummonObjects : MonoBehaviour
{
    private int summonCharId = 0;                   //ID
    private float summonDuration = 0;               //����
    private int summonAttack = 0;                   //�U����
    private int summonAttribute = 0;                //����
    private int summonHp = 0;                       //HP
    private float summonAttentionDamage = 0;        //��S�_���[�W
    private float summonAttentionRate = 0;          //��S��
    private float summonKnockBackValue = 0;         //�m�b�N�o�b�N��

    private float durationTimer = 0;                //�����^�C�}�[

    [SerializeField] GameObject playerObj;          //�v���C���[�I�u�W�F�N�g
    [SerializeField] Player playerScript;           //�v���C���[�̃X�N���v�g

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���g�̏���
        durationTimer += Time.deltaTime;
        if (summonDuration <= durationTimer)
        {
            Destroy(this.gameObject);
        }

    }

    //�������ꂽ��(���̎󂯎��Ȃ�)
    //�����L����ID�A����(�b)�A�U���́AHP�A��S�_���[�W�A��S��
    //�U���͂�HP�̓X�i�b�v�V���b�g
    //�U���̏ꍇ�͔{���v�Z��̍U���́A�����A��S�_���[�W�A��S���A�U�������ꏊ�A�U���͈͂̑傫��(x,y)�A�m�b�N�o�b�N��
    public void Summon(int charId, float duration, int attack, int attribute, int hp, float attentionDamage, float attentionRate, float knockBackValue)
    {
        //��ʐ�������Ȃ��̂�Find�͋C�ɂ��Ȃ��Ă悢
        playerObj = GameObject.Find("Player");
        playerScript = playerObj.GetComponent<Player>();

        summonCharId = charId;
        summonDuration = duration;
        summonAttack = attack;
        summonAttribute= attribute;
        summonKnockBackValue = knockBackValue;
        summonHp = hp;
        summonAttentionDamage = attentionDamage;
        summonAttentionRate = attentionRate;

        //�L����ID3�p�̏���
        char3();
        
    }

    //�U��
    public void Attack() 
    {
        //
        Vector2 attackRangePosition = this.transform.position;              //�U���ʒu��
        Vector2 attackRangeSize = this.transform.localScale * 10;            //�U���͈͉�
        playerScript.AttackMaker(summonAttack, summonAttribute, summonAttentionDamage, summonAttentionRate, attackRangePosition, attackRangeSize, summonKnockBackValue);
    }

    //��
    public void Heal(float heal) 
    {
        float healValue = heal;
        playerScript.Heal(healValue);
    }

    //�ȍ~�L�������Ƃ̏���
    //�L����ID3�̃X�L��(��ŏ�������)
    public void char3() 
    {
        StartCoroutine(char3skill());
    }

    //�L����ID3�̃X�L��timedeltatime�ɏ��������邱��
    IEnumerator char3skill() 
    {
        for (int i = 0; i < 20; i++) 
        {
            Attack();
            Heal((float)summonHp * 0.05f);
            for(int j = 0; j < 30; j++) 
            { 
                yield return null;
            }
        }

        yield break;
    }
}
