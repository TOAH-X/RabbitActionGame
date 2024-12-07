using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowObject : MonoBehaviour
{
    private int arrowCharId = 0;                    //ID
    private float arrowDuration = 0;                //����
    private int arrowAttack = 0;                    //�U����
    private int arrowAttribute = 0;                 //����
    private int arrowHp = 0;                        //HP
    private float arrowAttentionDamage = 0;         //��S�_���[�W
    private float arrowAttentionRate = 0;           //��S��
    private float arrowKnockBackValue = 0;          //�m�b�N�o�b�N��

    private bool isAttentionDamage = false;         //��S�_���[�W��

    private int arrowAttackType = 0;                //�L�������̖�̎�ށB�ʏ�U��:0�A���̑��̓L��������

    private Rigidbody2D rb2d;                       //rigidbody

    private Player playerScript;                    //�v���C���[�X�N���v�g

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�������ꂽ��(���̎󂯎��Ȃ�)
    //�v���C���[�X�N���v�g�A�L����ID�A�U���̃^�C�v(�ʏ�U��:0�A���̑��̓L�������ƂɊ��蓖��)�A�L�����{���v�Z��̍U���́A�����A��S�_���[�W�A��S���A�U�������ꏊ�A�U���͈͂̑傫��(x,y)�A�m�b�N�o�b�N��
    public void Arrow(Player player, int charId, int attackType, int attack, int attribute, float attentionDamage, float attentionRate, float knockBackValue)
    {
        //(����)�A�����A�d�́A�Ǐ]���邩�A�͈̓_���[�W�������������邱��
        arrowCharId = charId;
        arrowAttack = attack;
        arrowAttribute = attribute;
        arrowKnockBackValue = knockBackValue;
        arrowAttentionDamage = attentionDamage;
        arrowAttentionRate = attentionRate;
        arrowAttackType = attackType;

        rb2d = GetComponent<Rigidbody2D>();

        playerScript = player;

        //�L����ID4�p�̏���
        if (arrowCharId == 4 && attackType >= 1)
        {
            Char4();
        }
    }

    //���g�̏��ł̊Ǘ�
    public void ArrowDestroy() 
    {
        Destroy(gameObject);
    }

    //�ȍ~�L�������Ƃ̏���
    //(���������O��ς��邱��)
    public void Char4() 
    {
        

        rb2d.gravityScale = 0f;

        

        StartCoroutine(Char4skill());
    }

    //�L����ID4�̃X�L��
    IEnumerator Char4skill()
    {
        GameObject enemyObj = GameObject.FindWithTag("Enemy");      //�͈͓��ɑΉ������邱��

        float moveSpeed = -0.01f;

        float timer = 0;
        while (timer <= 2.0f)
        {
            if (enemyObj == null)
            {
                enemyObj = GameObject.FindWithTag("Enemy");
            }
            else
            {
                //�^�[�Q�b�g������
                float distance = ((Vector2)enemyObj.transform.position - (Vector2)transform.position).sqrMagnitude;
                Vector2 direciton = new Vector2((enemyObj.transform.position.x - transform.position.x), (enemyObj.transform.position.y - transform.position.y));
                if (timer <= 2f)
                {
                    transform.localEulerAngles = new Vector3(0, 0, -Mathf.Atan2(direciton.x, direciton.y) / Mathf.PI * 180 - (arrowAttackType - 5) * 12 * (2f - timer));
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, -Mathf.Atan2(direciton.x, direciton.y) / Mathf.PI * 180);
                }
            }
            //�ǔ�
            transform.position += (transform.up * moveSpeed) * 60 * Time.deltaTime;

            if (moveSpeed <= 5) 
            {
                moveSpeed += 0.003f * 60 * Time.deltaTime;
            }

            /*
            //�ǔ��Z�b�g
            //�^�[�Q�b�g������
            float distance = ((Vector2)enemyObj.transform.position - (Vector2)transform.position).sqrMagnitude;
            Vector2 direciton = new Vector2((enemyObj.transform.position.x - transform.position.x), (enemyObj.transform.position.y - transform.position.y));
            transform.localEulerAngles = new Vector3(0, 0, -Mathf.Atan2(direciton.x, direciton.y) / Mathf.PI * 180);
            //�ǔ�
            transform.position += (transform.up * moveSpeed);
            */

            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0;
        ArrowDestroy();
        yield break;
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

                //�͈͍U��
                //Vector2 attackRangePosition = this.transform.position;
                //Vector2 attackRangeSize = new Vector2(0.5f, 0.5f);
                //playerScript.AttackMaker(arrowAttack, arrowAttribute, arrowAttentionDamage, arrowAttentionRate, attackRangePosition, attackRangeSize, arrowKnockBackValue);

                //�P�̒��ڍU���p
                //��S���̒��I
                float randomPoint = Random.value * 100;
                if (randomPoint <= arrowAttentionRate)
                {
                    arrowAttack = (int)((float)(arrowAttack) * ((100 + arrowAttentionDamage) / 100));
                    isAttentionDamage = true;
                }
                //�U�������L������ID�A�_���[�W�����x���W�A�U���́A�����A��S���ǂ���
                enemyHpScript.EnemyDamage(arrowCharId, this.transform.position.x, arrowAttack, arrowAttribute, isAttentionDamage, arrowKnockBackValue);
                

                //���u��
                ArrowDestroy();
            }
        }
    }
}
