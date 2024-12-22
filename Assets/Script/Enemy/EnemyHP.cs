using System.Buffers.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] int enemyId = 1;                           //�G��ID
    [SerializeField] int enemyMaxHp = 100000;                   //�G�̍ő�HP
    [SerializeField] int enemyCurrentHp = 0;                    //�G�̌���HP
    [SerializeField] int enemyAttribute = 0;                    //�G�̑���

    //�_���[�W�\�L�p
    [SerializeField] GameObject damageNotationObj;              //�_���[�W�\�L�I�u�W�F�N�g
    [SerializeField] Transform canvasTransform;                 //Canvas��������(�_���[�W�\���p�Ȃ�)

    //���S���p
    [SerializeField] EnemyAction enemyActionScript;             //�G�l�~�[�A�N�V�����X�N���v�g(���S���p)
    [SerializeField] int latestAttackCharId = 0;                //�Ō�Ɏ��g���U�������L������ID(���S���p)

    private GameObject playerObj;                               //�v���C���[Obj(�Ώ��Ń_���[�W�p)���Ώ��Ń_���[�W�̓v���C���[��(�_���[�W��^�����)�ōs��

    [SerializeField] float knockBackGauge = 0;                  //�m�b�N�o�b�N�Q�[�W(���ꂪ�}�b�N�X�ɂȂ�Ɛ������)
    [SerializeField] float KnockBackLimit = 500;                //�m�b�N�o�b�N���~�b�g(���̒l�𒴂���ƃm�b�N�o�b�N����)�����f�̐��Œl��ύX�������ADB�Q��
    [SerializeField] float knockBackCoolTime = 60;              //�m�b�N�o�b�N�̃N�[���^�C��(deltaTime�ɕς��܂��傤)
    private float currentKnockBackCoolTime = 0;                 //���Ƀm�b�N�o�b�N������܂ł̎���

    private Enemy enemyScript;                                  //Enemy�X�N���v�g
    private EnemyHP enemyHpScript;                              //EnemyHP�X�N���v�g
    private Player playerScript;                                //Player�X�N���v�g

    [SerializeField] float debuffedAttributeResistance = 0;     //�����ϐ�

    // Start is called before the first frame update
    void Start()
    {
        enemyHpScript = GetComponent<EnemyHP>();

        //�X�e�[�^�X�Q��
        enemyScript = this.gameObject.GetComponent<Enemy>();
        enemyAttribute = enemyScript.EnemyAttribute;
        enemyMaxHp = enemyScript.EnemyMaxHp;

        //�X�e�[�^�X���(HP)
        enemyCurrentHp = enemyMaxHp;

        //Canvas��������(�_���[�W�\���p�Ȃ�)
        canvasTransform = GameObject.Find("Canvas").transform;
        //�v���C���[��������(�]�͂�����Όy�������ɏ��������邱��)
        playerObj = GameObject.Find("Player");
        playerScript = playerObj.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //���g��HP���}�C�i�X�ɂȂ����Ƃ�
        if (enemyCurrentHp < 0)
        {
            enemyCurrentHp = 0;
        }
        //���S����
        if (enemyCurrentHp == 0)
        {
            enemyActionScript.EnemyDeath(latestAttackCharId);
        }

        //�m�b�N�o�b�N�l�̌���
        if (knockBackGauge > 0) 
        {
            knockBackGauge--;
        }
        //�m�b�N�o�b�N�̃N�[���^�C��
        if (currentKnockBackCoolTime > 0) 
        {
            currentKnockBackCoolTime--;
        }
    }

    //��_���[�W����
    public void EnemyDamage(int attackCharId, float attackRangePos, int damage, int attribute, bool isAttentionDamage, float knockBackValue, bool isFollowUpAttack)
    {
        //�{��(���������邱��)
        float attributeResistance = (float)((float)GameSystemUtility.CalcDamage(damage, enemyAttribute, attribute, () => StartCoroutine(PairAnnihilationDamage(damage))) /(float) damage);
        //���̏����Ƒ�։\
        damage = GameSystemUtility.CalcDamage(damage, enemyAttribute, attribute, () => StartCoroutine(PairAnnihilationDamage(damage)));
        //�����ϐ��̔{������v�Z
        damage = Mathf.CeilToInt(damage * (attributeResistance + Mathf.Sqrt(debuffedAttributeResistance * (Mathf.Sqrt(1 / attributeResistance)))));

        //�����{������A���u��(�G�������ʂ̃X�N���v�g����邱��)
        //damage = AttributeCalculator(damage, attribute);
        enemyCurrentHp -= damage;

        //�_���[�W�\�L�Ăяo��
        var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
        DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
        damageNotationObjsScript.DamageNotion(damage, attribute, isAttentionDamage, (Vector2)transform.position + new Vector2(0, 0.0f));

        //�m�b�N�o�b�N�N�[���^�C��������Ƃ��̓m�b�N�o�b�N�l���~�ς��Ȃ�
        if (currentKnockBackCoolTime <= 0) 
        {
            knockBackGauge += knockBackValue;
        }

        //�m�b�N�o�b�N����
        if (knockBackGauge >= KnockBackLimit) 
        {
            bool isKnockBackRight;
            currentKnockBackCoolTime = knockBackCoolTime;
            //�E�ɐ������
            if (attackRangePos <= this.transform.position.x) 
            {
                isKnockBackRight = true;
                enemyActionScript.EnemyKnockBack(isKnockBackRight);
                Debug.Log("�E�Ƀm�b�N�o�b�N");
            }
            //���ɐ������
            else
            {
                isKnockBackRight = false;
                enemyActionScript.EnemyKnockBack(isKnockBackRight);
                Debug.Log("���Ƀm�b�N�o�b�N");
            }
            knockBackGauge = 0;
        }

        //�Ō�ɍU�������L����ID�̕ۑ�
        latestAttackCharId = attackCharId;

        //�ǌ�
        if (isFollowUpAttack == false) 
        {
            if (playerScript == null) 
            {
                playerScript = playerObj.gameObject.GetComponent<Player>();
            }
            if (playerScript != null)
            {
                playerScript.FollowUpAttack(enemyHpScript);
            }
            
        }

        //Debug.Log("�L����ID" + attackCharId + "�@�^�����_���[�W" + damage);
    }

    /*
    //�����{������A���u��(�G�������ʂ̃X�N���v�g����邱��)
    public int AttributeCalculator(int damage, int attribute) 
    {
        float damageRate = 1;

        //���g����
        if (enemyAttribute == 1)  
        {
            if (attribute == 4) 
            {
                damageRate = 2;
            }
            if (attribute == 3)
            {
                damageRate = 2;
            }
        }
        //���g����
        else if (enemyAttribute == 2)
        {
            if (attribute == 1)
            {
                damageRate = 2;
            }
        }
        //���g����
        else if (enemyAttribute == 3)
        {
            if (attribute == 2)
            {
                damageRate = 2;
            }
            if (attribute == 1)
            {
                damageRate = 2;
            }
        }
        //���g���y
        else if (enemyAttribute == 4)
        {
            if (attribute == 3)
            {
                damageRate = 2;
            }
        }
        //���g���G�[�e��
        else if (enemyAttribute == 5)
        {
            if (attribute == 1 || attribute == 2 || attribute == 3 || attribute == 4) 
            {
                damageRate = 1.5f;
            }
            if (attribute == 6)
            {
                //�Ώ��Ń_���[�W�𔭐�
                if (playerObj != null) 
                {
                    StartCoroutine(PairAnnihilationDamage(damage));
                }
            }
        }
        //���g������
        else if (enemyAttribute == 6)
        {
            if (attribute == 1 || attribute == 2 || attribute == 3 || attribute == 4)
            {
                damageRate = 0.5f;
            }
            if (attribute == 5)
            {
                damageRate = 2;
                //�Ώ��Ń_���[�W�𔭐�
                if (playerObj != null)
                {
                    StartCoroutine(PairAnnihilationDamage(damage));
                }
            }
        }

        return Mathf.CeilToInt((float)(damage * damageRate));
    }
    */

    //�Ώ��Ń_���[�W(0.1�b��ɔ���)
    IEnumerator PairAnnihilationDamage(int damage) 
    {
        for (int i = 0; i < 12; i++)  
        { 
            yield return null;
        }

        Player playerScript = playerObj.GetComponent<Player>();
        playerScript.AttackMaker(damage, 0, 0, 0, this.transform.position, new Vector2(5, 5), 1000, true);

        yield break;
    }

    //�z���p
    public void EnemyVacuum(float attackRangePos) 
    {
        bool isKnockBackRight;
        //�E�ɐ������
        if (attackRangePos >= this.transform.position.x)
        {
            isKnockBackRight = true;
            enemyActionScript.EnemyKnockBack(isKnockBackRight);
            Debug.Log("�E�ɋz����");
        }
        //���ɐ������
        else
        {
            isKnockBackRight = false;
            enemyActionScript.EnemyKnockBack(isKnockBackRight);
            Debug.Log("���ɋz����");
        }
    }

    //�����ϐ��_�E��
    public void DebuffedAttributeResistance(float attributeResistance, bool isTypeMoment, float duration, int debuffedCharId, int debuffedId)
    {
        debuffedAttributeResistance = attributeResistance;
        Debug.Log(debuffedAttributeResistance+"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
    }

    //enemyMaxHp�Q�Ɨp(getset)
    public int EnemyMaxHp // �v���p�e�B
    {
        get { return enemyMaxHp; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { enemyMaxHp = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }
    //enemyCurrentHp�Q�Ɨp(getset)
    public int EnemyCurrentHp // �v���p�e�B
    {
        get { return enemyCurrentHp; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { enemyCurrentHp = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }
}
