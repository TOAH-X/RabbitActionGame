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
        if (summonCharId == 3) 
        {
            Char3();
        }
        //�L����ID3�p�̏���
        if (summonCharId == 4)
        {
            Char4();
        }
    }

    //�U��
    //�U���́A���W�A�T�C�Y
    public void Attack(int attack, Vector2 attackRangePosition, Vector2 attackRangeSize)
    {
        playerScript.AttackMaker(attack, summonAttribute, summonAttentionDamage, summonAttentionRate, attackRangePosition, attackRangeSize, summonKnockBackValue, false);
    }

    //��
    public void Heal(float heal) 
    {
        float healValue = heal;
        playerScript.Heal(false, healValue);
    }

    //�ȍ~�L�������Ƃ̏���
    //�L����ID3�̃X�L��(��ŏ�������)
    public void Char3() 
    {
        StartCoroutine(Char3skill());
    }
    //�L����ID4�̃X�L��(��ŏ�������)
    public void Char4()
    {
        //���u���ő��z�炵���F�ɂ��Ă���
        this.GetComponent<SpriteRenderer>().color = new Color32(200, 50, 50, 60);
        StartCoroutine(Char4skill());
    }

    //�L����ID3�̃X�L��
    IEnumerator Char3skill() 
    {
        float timer = 0;
        for (int i = 0; i < 20; i++) 
        {
            Attack((int)(summonAttack * 1.35f), this.transform.position, this.transform.localScale * 10);
            Heal((float)summonHp * 0.20f);
            while (timer <= 0.5f) 
            {
                yield return null;
                timer += Time.deltaTime;
            }
            timer = 0;
        }
        yield break;
    }
    //�L����ID4�̃X�L��
    IEnumerator Char4skill()
    {
        float timer = 0;
        for (int i = 0; i < 50; i++)
        {
            Attack((int)(summonAttack * 0.35f), this.transform.position, this.transform.localScale * 1);
            while (timer <= 0.2f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            timer = 0;
        }
        yield break;
    }
}
