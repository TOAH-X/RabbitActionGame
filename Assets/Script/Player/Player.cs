using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;                  //Rigidbody2D
    [SerializeField] private SpriteRenderer spriteRenderer;     //SpriteRenderer
    [SerializeField] float moveSpeed = 3.0f;                    //�ړ����x
    [SerializeField] float baseMoveSpeed = 3.0f;                //��b�ړ����x
    [SerializeField] float jumpForce = 12.0f;                   //�W�����v��
    [SerializeField] float dashForce = 10.0f;                   //�_�b�V����
    [SerializeField] float hookShotMoveSpeed = 0.5f;            //�t�b�N�V���b�g�̈ړ����x

    [SerializeField] LayerMask groundLayer;                     //Layer��Ground
    [SerializeField] GameObject hookShotObj;                    //�t�b�N�V���b�g�̃v���n�u
    [SerializeField] GameObject normalAttackRangeObj;           //�ʏ�U���͈͂̃I�u�W�F�N�g
    [SerializeField] GameObject staminaNotationObj;             //�X�^�~�i�Q�[�W�̃I�u�W�F�N�g
    [SerializeField] GameObject playerSummonObjects;            //�v���C���[�������ł���I�u�W�F�N�g
    [SerializeField] GameObject vacuumRangeObj;                 //�z�����ݔ͈͂̃I�u�W�F�N�g
    [SerializeField] GameObject debuffedAttributeResistanceObj; //�f�o�t�I�u�W�F�N�g
    [SerializeField] GameObject playerArrowObj;                 //��(�Ȃǉ������p)�̃I�u�W�F�N�g

    [SerializeField] MainCameraController mainCameraControllerScript;   //���C���J�����̃X�N���v�g

    private bool isFollowUpAttack = false;                      //�ǌ��g���K�[(�L����5�p�Ȃ̂ŏ��������邱��)
    
    [SerializeField] List<GameObject> hookShotObjList = new List<GameObject>();     //�t�b�N�V���b�g�̃v���n�u���X�g
    [SerializeField] int hookShotObjCount = 0;                  //�t�b�N�V���b�g�̃J�E���g

    [SerializeField] GameObject afterEffectObj;                 //�c��

    [SerializeField] int staminaRecoverySpeed = 60;             //�X�^�~�i�񕜑��x(1�b(60�t���[��)�Ŋ�񕜂ł��邩)��1�t���[��1�񕜂����x�A�����傫���قǊɂ₩�ɉ�

    [SerializeField] GameObject hpGaugeObj;                     //HP�Q�[�W�̃I�u�W�F�N�g
    [SerializeField] GameObject mpGaugeObj;                     //MP�Q�[�W�̃I�u�W�F�N�g

    [SerializeField] TeamCoutnroller teamCoutnrollerScript;     //TeamCoutnroller�̃X�N���v�g

    [SerializeField] float invincibilityTimer = 0;              //���G���ԊǗ�
    [SerializeField] float invincibilityCoolTime = 0.25f;       //���G����
    [SerializeField] float dashTimer = 0;                       //�_�b�V�����ԊǗ�(3��ȏ�A���Ń_�b�V�������Ȃ�)

    [SerializeField] float pairAnnihilationDamageCoolTime = 0.1f;   //�Ώ��Ń_���[�W�̃N�[���^�C��
    [SerializeField] float pairAnnihilationDamageTimer = 0;     //�Ώ��Ń_���[�W�̃N�[���^�C��

    [SerializeField] const float knockBackTime = 0.3f;          //�m�b�N�o�b�N���󂯂���̍d������
    private float currentKnockBackTime = 0f;                    //���݂̃m�b�N�o�b�N�̍d������
    private bool isStun = false;                                //�d�����Ă��邩

    [Header("�L����ID")]
    [SerializeField] int charId = 1;                            //�L�����N�^�[��ID
    [Header("����")]
    [SerializeField] int attribute = 0;                         //����(-1:�񕜁A0:������(����)�A1:�΁A2:���A3:���A4:�y�A5:�G�[�e���A6:����(�P�m��))
    [Header("�����")]
    [SerializeField] int weaponType = 0;                        //�����
    [Header("���x��")]
    [SerializeField] byte charLevel = 1;                        //�L�������x��
    [Header("�o���l��")]
    [SerializeField] int charExp = 0;                           //�L�����o���l
    [Header("�ő�X�^�~�i")]
    [SerializeField] int maxStamina = 100;                      //�ő�X�^�~�i
    [Header("���݃X�^�~�i")]
    [SerializeField] int currentStamina;                        //���݃X�^�~�i
    [Header("��bHP")]
    [SerializeField] int baseHp = 100;                          //��bHP
    [Header("�ő�HP")]
    [SerializeField] int maxHp = 100;                           //�ő�HP
    [Header("����HP")]
    [SerializeField] int currentHp;                             //����HP
    [Header("�ő�MP")]
    [SerializeField] int maxMp = 100;                           //�ő�MP
    [Header("����MP")]
    [SerializeField] int currentMp;                             //����MP
    [Header("��b�U����")]
    [SerializeField] int baseAttack;                            //��b�U����
    [Header("�U����(�ŏI�I)")]
    [SerializeField] int attack = 740;                          //�U����
    [Header("��S�_���[�W")]
    [SerializeField] float attentionDamage = 200;               //��S�_���[�W(�U��+��S�_���[�W(��))��)��S�_���[�W150%�͒ʏ��2.5�{
    [Header("��S��")]
    [SerializeField] float attentionRate = 30;                  //��S��(�����100%)
    [Header("�K�E�Z�̃R�X�g")]
    [SerializeField] int maxSpecialMoveEnergy = 100;            //�K�E�Z�̃R�X�g
    [Header("���݂̕K�E�Z�̃G�l���M�[��")]
    [SerializeField] int currentSpecialMoveEnergy = 100;        //���݂̕K�E�Z�̃R�X�g���ǂ̂��炢���܂��Ă��邩
    [Header("�K�E�Z�̃N�[���^�C��")]
    [SerializeField] float maxSpecialMoveRecharge = 10.0f;      //�K�E�Z�̃N�[���^�C���ݒ�(�b)
    [Header("�K�E�Z�̃N�[���^�C���̎c�莞��")]
    [SerializeField] float currentSpecialMoveRecharge = 0;      //���݂̕K�E�Z�̃N�[���^�C��(���ɃX�L�����g����܂ł̎���)
    [Header("�X�L���̃N�[���^�C��")]
    [SerializeField] float maxSkillRecharge = 10.0f;            //�X�L���̃N�[���^�C���ݒ�(�b)
    [Header("�X�L���̃N�[���^�C���̎c�莞��")]
    [SerializeField] float currentSkillRecharge = 0;            //���݂̃X�L���̃N�[���^�C��(���ɃX�L�����g����܂ł̎���)

    //�U���Ɋւ��āA�ђʂ��邩�ƍU���̎����̍��ڂ������邱��(NormalAttackRange�ł̐U�镑����ύX����)
    //�U�����֐��ɂ��Ă����A�K�v�Ȉ�����Z�߂邱��

    [Header("��b�U���o�t")]
    [SerializeField] int baseAttackBuff = 0;                    //��b�U���o�t(���l)
    [Header("�U���o�t")]
    [SerializeField] float attackBuff = 1.0f;                   //�U���o�t(��)
    [Header("HP�o�t")]
    [SerializeField] float hpBuff = 1.0f;                       //HP�o�t(��)
    [Header("�_���[�W�o�t")]
    [SerializeField] float damageBuff = 1.0f;                   //�_���[�W�o�t(��)

    [SerializeField] float damageReductionRate = 0f;            //�_���[�W�y����

    //Update�֐����Ńo�t�̌v�Z�𖈃t���[���s������
    //�����o�t�������邱��
    private bool isLookRight = true;                            //��������(�E�A�O�������Ă��邩)
    private bool isHookShotMoving = false;                      //�t�b�N�V���b�g�ňړ�����
    private bool isDash = false;                                //�_�b�V������

    private int staminaRecoveryCounter = 0;                     //�X�^�~�i�񕜏����̃J�E���^�[

    //�_���[�W�\�L�p
    [SerializeField] GameObject damageNotationObj;              //�_���[�W�\�L�I�u�W�F�N�g
    [SerializeField] GameObject canvasObj;                      //Canvas��������(�_���[�W�\���p�Ȃ�)
    [SerializeField] Transform canvasTransform;                 //Canvas�̍��W��(�_���[�W�\���p�Ȃ�)

    //�ȍ~�L�������L�̕ϐ�

    //�L����ID1�̎�l���p
    [SerializeField] int normalAttackAttribute = 0;             //�ʏ�U���̑���
    //�L����ID2�̃V�����p
    [SerializeField] bool isChar2Attention = false;             //�m���S

    //�ȏ�L�������L�̕ϐ��ł���

    //�L�����N�^�[�f�[�^�x�[�X
    public DB_CharData dB_charData;

    // Start is called before the first frame update
    void Start()
    {
        //FPS�Œ�
        Application.targetFrameRate = 60;

        //�X�e�[�^�X���f�[�^�x�[�X����Q��
        CharDbReference();

        //�X�e�[�^�X���
        currentHp = maxHp;
        currentMp = maxMp;
        currentStamina = maxStamina;

        //Canvas��������(�_���[�W�\���p�Ȃ�)
        canvasObj = GameObject.Find("DamageCanvas");
        canvasTransform = canvasObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //FPS�v��
        //float fps = 1f / Time.deltaTime;
        //Debug.Log("FPS:" + fps);

        //�U���͌v�Z
        attack = Mathf.CeilToInt((float)((baseAttack + baseAttackBuff) * attackBuff) * damageBuff);
        //HP�v�Z
        maxHp = Mathf.CeilToInt(baseHp * hpBuff);
        //��b�U���̃��t���b�V��
        baseAttackBuff = 0;
        //�U���̓o�t�̃��t���b�V��
        attackBuff = 1.0f;
        //HP�o�t�̃��t���b�V��
        hpBuff = 1.0f;
        //�_���[�W�o�t�̃��t���b�V��
        damageBuff = 1.0f;
        //�_���[�W�y�����̃��t���b�V��
        damageReductionRate = 0;

        //����ړ�(�t�b�N�V���b�g)���ł͂Ȃ��Ƃ�
        if (isHookShotMoving == false)
        {
            //�_�b�V������
            if (isDash == false) 
            {
                //�ړ�
                MoveUpdate();
                //�_�b�V��
                Dash();
            }

            //����ړ�(�t�b�N�V���b�g)...�ꕔ�L�����̓���ړ��H�X�L���ɂ���\��
            HookShot();

            //�ʏ�U��
            NormalAttack(attack, normalAttackAttribute);           //�U���͈ˑ�

            //Ray�̔���(�n��ɂ���Ƃ�)
            if (IsGrounding() == true)
            {
                //�W�����v
                JumpUpdate();
            }
        }

        //����
        Characteristic();

        //�K�E�Z
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            SpecialMove();
        }
        //�X�L��
        if (Input.GetKeyDown(KeyCode.E))
        {
            Skill();
        }

        //���S��
        if (CurrentHp == 0)
        {
            PlayerDeath();
        }

        //�X�^�~�i��(���R)
        Stamina();

        //���G����
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        //�_�b�V���\����
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        //�_�b�V���\����
        if (pairAnnihilationDamageTimer > 0)
        {
            pairAnnihilationDamageTimer -= Time.deltaTime;
        }

        //�X�L���N�[���^�C��
        if (currentSkillRecharge > 0)
        {
            currentSkillRecharge -= Time.deltaTime;
        }
        //�K�E�Z�N�[���^�C��
        if (currentSpecialMoveRecharge > 0)
        {
            currentSpecialMoveRecharge -= Time.deltaTime;
        }

        //�l���}�C�i�X�ɂȂ����Ƃ�0�ɂ���
        if (currentHp < 0)
        {
            currentHp = 0;
        }
        if (currentMp < 0)
        {
            currentMp = 0;
        }
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
        if (currentSkillRecharge < 0)
        {
            currentSkillRecharge = 0;
        }
        if (currentSpecialMoveRecharge < 0)
        {
            currentSpecialMoveRecharge = 0;
        }
        //�l���ő�l�𒴂����Ƃ��A�K��l�ȉ��ɂ��遦�Ȃ�ׂ��Ăяo����邱�Ƃ��Ȃ��悤��
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
            Debug.Log("HP�I�[�o�[");
        }
        if (currentMp > maxMp)
        {
            currentMp = maxMp;
            Debug.Log("MP�I�[�o�[");
        }
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
            Debug.Log("�X�^�~�i�I�[�o�[");
        }

        //�ޗ��ɗ������ꍇ�̈ړ�
        if (this.transform.position.y <= -100) 
        {
            rb2D.velocity = Vector3.zero;
            this.transform.position = new Vector2(0, 0);
        }

        //�X�^�~�i�Q�[�W�̒Ǐ]
        staminaNotationObj.transform.position = this.transform.position + new Vector3(0, -1.5f, 0);
    }

    //�ړ�����
    private void MoveUpdate()
    {
        //�ʏ�ړ�
        float moveX = Input.GetAxis("Horizontal"); // ���������̓��� (A�Ɓ��AD�Ɓ�)

        Vector2 moveDirection = new Vector2(moveX, 0);//�񎟌��x�N�g�����쐬

        if (moveDirection.magnitude > 0.1f) // ���͂����ȏ�̏ꍇ�̂ݍX�V
        {
            moveDirection.Normalize(); // �΂߈ړ�����葬�x�ɂ��邽�߂ɐ��K��
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90; // ��]�p�x���v�Z
            transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World); // �ړ������ɉ����Ĉړ�
        }
        //�����̕ύX
        if (moveX > 0)
        {
            Direction(true);
        }
        else if (moveX < 0)
        {
            Direction(false);
        }

        /*
        //���d�͈ړ�
        float moveX = Input.GetAxis("Horizontal"); // ���������̓��� (A�Ɓ��AD�Ɓ�)
        float moveY = Input.GetAxis("Vertical");   // ���������̓��� (W�Ɓ��AS�Ɓ�)

        Vector2 moveDirection = new Vector2(moveX, moveY);//�񎟌��x�N�g�����쐬

        if (moveDirection.magnitude > 0.1f) // ���͂����ȏ�̏ꍇ�̂ݍX�V
        {
            moveDirection.Normalize(); // �΂߈ړ�����葬�x�ɂ��邽�߂ɐ��K��
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90; // ��]�p�x���v�Z
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World); // �ړ������ɉ����Ĉړ�
        }
        */
    }

    //�����̕ύX
    private void Direction(bool isLookRightInDirection)
    {
        isLookRight = isLookRightInDirection;
        Vector3 playerScale = this.transform.localScale;

        //�X�^�~�i�Q�[�W�𔽓]�����Ȃ�
        //Transform staminaGaugeBaseTransform = this.transform.Find("StaminaGaugeBase");
        //Vector3 staminaGaugeBaseScale = staminaGaugeBaseTransform.localScale;

        //�E����
        if (isLookRight == true)
        {
            playerScale.x = 1 * Mathf.Abs(playerScale.x);
            //staminaGaugeBaseScale.x = 1 * Mathf.Abs(staminaGaugeBaseScale.x);
            //�t���Ɉړ����Ă����ꍇ�͎~�܂�
            if (rb2D.velocity.x < 0)
            {
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }
        }
        //������
        else
        {
            playerScale.x = -1 * Mathf.Abs(playerScale.x);
            //staminaGaugeBaseScale.x = -1 * Mathf.Abs(staminaGaugeBaseScale.x);
            //�t���Ɉړ����Ă����ꍇ�͎~�܂�
            if (rb2D.velocity.x > 0)
            {
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }
        }

        //staminaGaugeBaseTransform.localScale = staminaGaugeBaseScale;
        this.transform.localScale = playerScale;
    }

    //�W�����v����
    private void JumpUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //�ʏ�W�����v
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    //�n�ʂƂ̐ڒn����
    private bool IsGrounding()
    {
        float rayLength = 0.6f;
        Vector2 direction = transform.right * 0.8f;
        Vector2 startPos = transform.position + new Vector3(-0.4f, -1.05f, 0);

        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, rayLength, groundLayer);
        Debug.DrawRay(startPos, direction, Color.red);

        return hit.collider != null;
    }

    //�_�b�V��
    private void Dash()
    {
        if ((Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.LeftShift)) && currentStamina > 10 && dashTimer <= 0)  
        {
            //�X�^�~�i����
            ExhaustStamina(10);
            //���G
            InvincibilityTimer(0.25f);
            //�_�b�V���\���Ԃ̍X�V
            dashTimer = 0.25f;
            /*
            if (isLookRight == true)
            {
                rb2D.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
            }
            else
            {
                rb2D.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
            }
            */

            Vector2 moveDirection = Vector2.zero;
            /*
            if (Input.GetKey(KeyCode.W)) 
            { 
                moveDirection = Vector2.up;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveDirection = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDirection = Vector2.down;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveDirection = Vector2.right;
            }
            else 
            {
                if (isLookRight == true) 
                {
                    moveDirection = Vector2.right;
                }
                else 
                {
                    moveDirection = Vector2.left;
                }
            }
            */
            //8�����Ή�
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveX, moveY).normalized;
            if (moveDirection.magnitude == 0) 
            {
                if (isLookRight == true)
                {
                    moveDirection = Vector2.right;
                }
                else
                {
                    moveDirection = Vector2.left;
                }
            }

            StartCoroutine(MoveDash(moveDirection));
        }
    }
    //�_�b�V��
    IEnumerator MoveDash(Vector2 moveDirection)
    {
        isDash = true;
        float thisGravity = rb2D.gravityScale;
        rb2D.gravityScale = 0;
        Vector2 thisVerocity = rb2D.velocity;
        rb2D.velocity = Vector2.zero;
        //������
        Color thisColor = spriteRenderer.color;
        spriteRenderer.color = new Color32(0, 0, 0, 0);

        float timer = 0;
        int counter = 0;

        while (timer <= 0.2f)  
        {
            /*
            if (moveDirection==Vector2.up)
            {
                if (rb2D.velocity.y <= 10) 
                {
                    rb2D.AddForce(Vector2.up * dashForce, ForceMode2D.Impulse);
                }
                //rb2D.velocity = Vector2.right * dashForce;
            }
            else if (moveDirection == Vector2.left)
            {
                if (rb2D.velocity.x >= -10)
                {
                    rb2D.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
                }
            }
            else if (moveDirection == Vector2.down)
            {
                if (rb2D.velocity.y >= -10)
                {
                    rb2D.AddForce(Vector2.down * dashForce, ForceMode2D.Impulse);
                }
            }
            else if (moveDirection == Vector2.right)
            {
                if (rb2D.velocity.x <= 10)
                {
                    rb2D.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
                }
            }
            */
            if (rb2D.velocity.magnitude <= 10) 
            {
                rb2D.AddForce(moveDirection * dashForce, ForceMode2D.Impulse);
            }

            if (counter % 2 == 0) 
            {
                //�c���̐���(GetComponent���Ȃ�Ƃ�����)
                var afterEffectObj = Instantiate(this.afterEffectObj, this.transform.position, this.transform.rotation);
                afterEffectObj.transform.localScale = this.transform.localScale;
                SpriteRenderer afterEffectObjsSpriteRenderer = afterEffectObj.GetComponent<SpriteRenderer>();
                afterEffectObjsSpriteRenderer.color = new Color32(50, 150, 200, 100);
                Destroy(afterEffectObj, 0.25f);
            }

            timer += Time.deltaTime;
            counter++;
            yield return null;
        }

        //����������
        spriteRenderer.color = thisColor;
        
        rb2D.gravityScale = thisGravity;
        //rb2D.velocity *= 0.1f;
        //rb2D.velocity += thisVerocity;
        rb2D.velocity = Vector2.zero;

        isDash = false;
        
        //�󒆂ɂ���ۂ͏�����ɉ���
        if (IsGrounding() == false) 
        {
            rb2D.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
        }

        yield break;
    }

    //����ړ�(�t�b�N�V���b�g)�̏���
    private void HookShot()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.T) && currentStamina > 20)
        {
            //�X�^�~�i����
            ExhaustStamina(20);

            //�ړ���̖ڕW�I�u�W�F�N�g�̍��W�擾
            Vector2 mousePos = Input.mousePosition;
            //Debug.Log(mousePos);
            Camera camera = Camera.main;
            Vector2 touchScreenPos = camera.ScreenToWorldPoint(mousePos);
            //����
            GameObject hookShotObjs = Instantiate(hookShotObj) as GameObject;
            hookShotObjs.transform.position = touchScreenPos;
            hookShotObjs.transform.rotation = Quaternion.identity;
            hookShotObjList.Add(hookShotObjs);
            hookShotObjCount++;

            //����ړ�(�t�b�N�V���b�g)�̈ړ�����
            StartCoroutine(MoveHookShot(touchScreenPos));
        }
    }
    //����ړ�(�t�b�N�V���b�g)�̈ړ�����
    IEnumerator MoveHookShot(Vector2 touchScreenPos)
    {
        //�����̌v�Z
        Vector2 distance = (touchScreenPos - (Vector2)transform.position).normalized;

        //Debug.Log("distance" + distance + "touch" + touchScreenPos + "pos" + transform.position);

        //�X�^�e�B�b�N�ɕύX(�����������󂯕t���Ȃ�)
        rb2D.bodyType = RigidbodyType2D.Static;
        //rb.bodyType = RigidbodyType2D.Kinematic;
        //����ړ���ԂɕύX
        isHookShotMoving = true;

        //�����̕ύX
        if (distance.x > 0)
        {
            Direction(true);
        }
        else
        {
            Direction(false);
        }

        //�ڕW�n�_�܂ňړ�
        while (Vector2.Distance(touchScreenPos, (Vector2)transform.position) > 0.5)
        {
            transform.position += (Vector3)distance * hookShotMoveSpeed;
            yield return null;
            //�����̌v�Z
            distance = (touchScreenPos - (Vector2)transform.position).normalized;
        }
        //�X�^�e�B�b�N����߂�(�����������󂯕t����悤��)
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        //���x��0�ɕύX
        rb2D.velocity = Vector2.zero;
        //������ɉ���
        rb2D.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
        //����ړ���Ԃ�����
        isHookShotMoving = false;
        //�ړ��ڕW��̃I�u�W�F�N�g�̔j��
        if (hookShotObj == true)
        {
            Destroy(hookShotObjList[hookShotObjCount - 1]);
            hookShotObjList.RemoveAt(hookShotObjCount - 1);
            hookShotObjCount--;
            //Destroy(hookShotObj);
        }

        yield break;
    }

    //�U���͈͂̐���(�ʏ��X�L���A�K�E�Z�ȂǊ܂ޑS��)
    //�L����ID�A�{���v�Z��̍U���́A�����A��S�_���[�W�A��S���A�U�������ꏊ�A�U���͈͂̑傫��(x,y)�A�m�b�N�o�b�N��(charID�������Ă���̂ŉ����邱��)�A�ǌ���(�ǌ��Ȃ�true�A����ȊO��false)
    public void AttackMaker(int multipliedAttack, int finalAttributeNormalAttack, float multipliedAttentionDamage, float multipliedAttentionRate, Vector3 attackPos, Vector2 attackSize, float knockBackValue, bool isFollowUpAttack)
    {
        //char2���m���S�̂Ƃ�(�ړ������邱��)
        if (isChar2Attention == true)
        {
            multipliedAttentionRate = 100;
            isChar2Attention = false;
        }

        var normalAttackRangeObjs = Instantiate(normalAttackRangeObj, attackPos, this.transform.rotation);
        normalAttackRangeObjs.transform.localScale = new Vector3(attackSize.x, attackSize.y, 1.0f);
        NormalAttackRange normalAttackRangeObjsScript = normalAttackRangeObjs.GetComponent<NormalAttackRange>();
        normalAttackRangeObjsScript.NormalAttack(charId, multipliedAttack, finalAttributeNormalAttack, multipliedAttentionDamage, multipliedAttentionRate, knockBackValue, isFollowUpAttack);
    }

    //�ǌ�(PlayerArrowObjects�ɂ��P�̍U��������̂�Player���̊֐��œZ�߂邱��)
    public void FollowUpAttack(EnemyHP enemyHpScript) 
    {
        //OnFollowUpAttack?.Invoke(onFollowUpAttackMultipliedAttack, onFollowUpAttackFinalAttributeNormalAttack, onFollowUpAttackMultipliedAttentionDamage, onFollowUpAttackMultipliedAttentionRate, onFollowUpAttackAttackPos, onFollowUpAttackAttackSize, onFollowUpAttackKnockBackValue, onFollowUpAttackIsFollowUpAttack);
        if (isFollowUpAttack == true) //�U���͂��Œ�ɂ��邱��
        {
            //�ėp
            //SingleAttack(enemyHpScript, multipliedAttack, finalAttribute, multipliedAttentionDamage, multipliedAttentionRate, knockBackValue, isFollowUpAttack);
            SingleAttack(enemyHpScript, (int)(attack * 0.3f), 6, attentionDamage, attentionRate, 0, true);
        }
    }

    //�P�̍U��
    public void SingleAttack(EnemyHP enemyHpScript, int multipliedAttack, int finalAttribute, float multipliedAttentionDamage, float multipliedAttentionRate, float knockBackValue, bool isFollowUpAttack) 
    {
        bool isAttentionDamage = false;         //��S�_���[�W��
        //��S���̒��I
        float randomPoint = UnityEngine.Random.value * 100;
        if (randomPoint <= multipliedAttentionRate)
        {
            multipliedAttack = (int)((float)(multipliedAttack) * ((100 + multipliedAttentionDamage) / 100));
            isAttentionDamage = true;
        }
        //�U�������L������ID(�����ɂ���Ȃ�charId��ǐՂ��Ď󂯎���Ă��ǂ�)�A�_���[�W�����x���W�A�U���́A�����A��S���ǂ����A�ǌ����ǂ���(�ǌ���true)
        enemyHpScript.EnemyDamage(charId, this.transform.position.x, multipliedAttack, finalAttribute, isAttentionDamage, knockBackValue, isFollowUpAttack);
    }

    //��
    public void Heal(bool isTeamHeal, float healValue) 
    {
        //�S�̉�
        if (isTeamHeal == true)
        {
            for (int i = 0; i < 3; i++)
            {
                if (teamCoutnrollerScript.TeamCurrentHp[i] != 0)
                {
                    //�����̉�
                    if (charId == teamCoutnrollerScript.TeamIdData[i])
                    {
                        if (currentHp + Mathf.CeilToInt(healValue) >= maxHp)
                        {
                            currentHp = maxHp;
                        }
                        else
                        {
                            currentHp += Mathf.CeilToInt(healValue);
                        }
                        //�_���[�W�\�L�Ăяo��
                        var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
                        DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
                        //�����͍U���́A�����A��S����(��)�A�\�L�̔����ꏊ(�����̔�_���[�W�ɉ�S�_���[�W�͔������Ȃ�)
                        damageNotationObjsScript.DamageNotion(Mathf.CeilToInt(healValue), -1, false, (Vector2)transform.position + new Vector2(0, 0.0f));
                    }
                    //�����ȊO�̉�
                    else
                    {
                        if (teamCoutnrollerScript.TeamCurrentHp[i] + Mathf.CeilToInt(healValue) >= teamCoutnrollerScript.TeamMaxHp[i])
                        {
                            teamCoutnrollerScript.TeamCurrentHp[i] = teamCoutnrollerScript.TeamMaxHp[i];
                        }
                        else
                        {
                            teamCoutnrollerScript.TeamCurrentHp[i] += Mathf.CeilToInt(healValue);
                        }
                        //�_���[�W�\�L�Ăяo��
                        var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
                        DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
                        //�����͍U���́A�����A��S����(��)�A�\�L�̔����ꏊ(�����̔�_���[�W�ɉ�S�_���[�W�͔������Ȃ�)
                        damageNotationObjsScript.DamageNotion(Mathf.CeilToInt(healValue), -1, false, (Vector2)transform.position + new Vector2(0, 0.0f));
                    }
                }
            }
        }
        //�P�̉�
        else
        {
            //�����̉�
            if (currentHp + Mathf.CeilToInt(healValue) >= maxHp)
            {
                currentHp = maxHp;
            }
            else
            {
                currentHp += Mathf.CeilToInt(healValue);
            }
            //�_���[�W�\�L�Ăяo��
            var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
            DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
            //�����͍U���́A�����A��S����(��)�A�\�L�̔����ꏊ(�����̔�_���[�W�ɉ�S�_���[�W�͔������Ȃ�)
            damageNotationObjsScript.DamageNotion(Mathf.CeilToInt(healValue), -1, false, (Vector2)transform.position + new Vector2(0, 0.0f));
        }
    }

    //�W�G(�ŗL�X�L���ȂǂȂ̂Ń��t�@�N�^�����O�ł͕ʃX�N���v�g�ɂ��Ă�������)
    //�W�G�����ꏊ�A�W�G�͈́A�W�G��������(�b)
    public void Vacuum(Vector2 vacuumPos, Vector2 vacuumSize, float vacuumDuration, float VacuumPower) 
    {
        //�z������
        var vacuumRangeObjs = Instantiate(vacuumRangeObj, vacuumPos, this.transform.rotation);
        vacuumRangeObjs.transform.localScale = new Vector3(vacuumSize.x, vacuumSize.y, 1.0f);
        VacuumRange vacuumRangeObjsScript = vacuumRangeObjs.GetComponent<VacuumRange>();
        vacuumRangeObjsScript.Vacuum(vacuumDuration, VacuumPower);
    }

    //�����ϐ��_�E��
    //�ϐ��_�E���l(%)�A�����ʒu�A�f�o�t�͈́A�������Ԃ��f�o�t(���Ԉˑ�����u�Ŋ|����true)���f�o�t�͈͂�(�o���������flase)�A�������ԁA�U��ID
    public void DebuffedAttributeResistance(float debuffedAttributeResistance, Vector2 pos, Vector2 size, bool isTypeMoment, float duration, int debuffedId) 
    {
        //���������ϊ�
        debuffedAttributeResistance /= 100;
        //�ϐ��_�E��
        var debuffedAttributeResistanceObjs = Instantiate(debuffedAttributeResistanceObj, pos, this.transform.rotation);
        debuffedAttributeResistanceObjs.transform.localScale = new Vector3(size.x, size.y, 1.0f);
        DebuffedAttributeResistance debuffedAttributeResistanceObjsScript = debuffedAttributeResistanceObjs.GetComponent<DebuffedAttributeResistance>();
        debuffedAttributeResistanceObjsScript.Debuffed(debuffedAttributeResistance, isTypeMoment, duration, charId, debuffedId);
    }

    //�v���C���[�̏�����(�ݒu�X�L��)
    //�p������(summonDuration)�A�m�b�N�o�b�N�ʁA�����ʒu�A�T�C�Y
    public void PlayerSummonObjects(float summonDuration, float summonKnockBackValue, Vector2 playerSummonObjsPos, Vector2 playerSummonObjsSize)
    {
        //�ݒu
        var playerSummonObjs = Instantiate(playerSummonObjects, playerSummonObjsPos, this.transform.rotation);
        playerSummonObjs.transform.localScale = new Vector3(playerSummonObjsSize.x, playerSummonObjsSize.y, 1.0f);
        PlayerSummonObjects playerSummonObjectsScript = playerSummonObjs.GetComponent<PlayerSummonObjects>();
        playerSummonObjectsScript.Summon(charId, summonDuration, attack, attribute, maxHp, attentionDamage, attentionRate, summonKnockBackValue);
    }

    //�L����ID�A�{���v�Z��̍U���́A�����A��S�_���[�W�A��S���A�U�������ꏊ�A�U���͈͂̑傫��(x,y)�A�m�b�N�o�b�N��(charID�������Ă���̂ŉ����邱��)
    //public void AttackMaker(int multipliedAttack, int finalAttributeNormalAttack, float multipliedAttentionDamage, float multipliedAttentionRate, Vector3 attackPos, Vector2 attackSize, float knockBackValue)
    //�v���C���[�̖�(�������U��)
    //�U���^�C�v(�ʏ�0�AID�I�Ȃ���)�A�U���́A�����A��S�_���[�W�A��S���A��̔����ʒu�A��̑傫���A�U���͈�(0�̏ꍇ�͒P�̍U��)�A�m�b�N�o�b�N�ʁA���ˊp�x
    public void Arrow(int attackType, int multipliedAttack, int arrowAttribute, float arrowAttentionDamage, float arrowAttentionRate, Vector2 playerArrowObjsPos, Vector2 playerArrowObjsSize, Vector2 arrowAttackRangeSize, float arrowKnockBackValue, float arrowLaunchAngle)
    {
        //�ݒu
        var playerArrowObjs = Instantiate(playerArrowObj, playerArrowObjsPos, this.transform.rotation);
        playerArrowObjs.transform.localScale = new Vector3(playerArrowObjsSize.x, playerArrowObjsSize.y, 1.0f);
        PlayerArrowObject playerArrowObjectScript = playerArrowObjs.GetComponent<PlayerArrowObject>();
        playerArrowObjectScript.Arrow(GetComponent<Player>(), charId, attackType, multipliedAttack, arrowAttribute, arrowAttentionDamage, arrowAttentionRate, arrowAttackRangeSize, arrowKnockBackValue, arrowLaunchAngle);
    }

    //�ʏ�U��(�U����,����)
    private void NormalAttack(int attack, int attributeNormalAttack)
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.E) == false)
        {
            //�E�����ƍ������Ŕ����ʒu��ύX
            float normalAttackDirection = 0.75f;
            normalAttackDirection *= GetFacingDirection(isLookRight);
            Vector3 normalAttackPos = this.transform.position;
            normalAttackPos.x += normalAttackDirection;

            AttackMaker(attack, attributeNormalAttack, attentionDamage, attentionRate, normalAttackPos, new Vector2(2.0f, 2.0f), 100, false);
        }
    }

    //�K�E�Z
    public void SpecialMove()
    {
        if (currentSpecialMoveRecharge <= 0 && isHookShotMoving == false) 
        {
            currentSpecialMoveRecharge = maxSpecialMoveRecharge;

            if (charId == 1)
            {
                Char1SpecialMove();
            }
            else if (charId == 2)
            {
                Char2SpecialMove();
            }
            else if (charId == 3)
            {
                Char3SpecialMove();
            }
            else if (charId == 4)
            {
                Char4SpecialMove();
            }
            else if (charId == 5)
            {
                Char5SpecialMove();
            }
            else if (charId == 6)
            {
                Char6SpecialMove();
            }
            else if (charId == 7)
            {
                Char7SpecialMove();
            }
            else if (charId == 8)
            {
                Char8SpecialMove();
            }
        }
    }

    //�L����ID��1�̃L�����̕K�E�Z
    async public void Char1SpecialMove()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i <= 1)
            {
                AttackMaker((int)(attack * 2.4f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(12.5f, 12.5f), 120, false);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
            else
            {
                AttackMaker((int)(attack * 0.8f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(12.5f, 12.5f), 40, false);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
    }

    //�L����ID��2�̃L�����̕K�E�Z
    async public void Char2SpecialMove()
    {
        rb2D.AddForce(new Vector2(0,7.5f), ForceMode2D.Impulse);
        
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        
        rb2D.bodyType = RigidbodyType2D.Static;
        //�Z�b�|����1��]�̂悤�ɋL�q
        /*
        float angle = 360;
        float timer = 0;
        while (timer < 1)
        {
            this.transform.Rotate(0, 0, -angle * Time.deltaTime * GetFacingDirection(isLookRight));
            timer += Time.deltaTime;
            yield return null;
        }
        */
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        for (int i = 0; i < 10; i++)
        {
            if (i == 0)
            {
                AttackMaker((int)(attack * 12.5f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(7.5f, 7.5f), 1200, false);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
            else
            {
                AttackMaker((int)(attack * 0.2f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(15.0f, 7.5f), 10, false);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        }
        rb2D.bodyType = RigidbodyType2D.Dynamic;
    }

    //�L����ID��3�̃L�����̕K�E�Z
    public void Char3SpecialMove()
    {
        PlayerSummonObjects(10, 0, this.transform.position, new Vector2(0.5f, 0.5f));
    }

    //�L����ID��4�̃L�����̕K�E�Z
    public void Char4SpecialMove()
    {
        PlayerSummonObjects(10, 0, this.transform.position, new Vector2(15.0f, 15.0f));
    }

    //�L����ID��5�̃L�����̕K�E�Z
    async public void Char5SpecialMove()
    {
        AttackMaker((int)(attack * 3.6f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(7.5f, 7.5f), 300, false);

        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

        isFollowUpAttack = true;
        
        await UniTask.Delay(TimeSpan.FromSeconds(15.0f));

        isFollowUpAttack = false;
    }

    //�L����ID��6�̃L�����̕K�E�Z
    async public void Char6SpecialMove()
    {
        DebuffedAttributeResistance(20, transform.position, new Vector2(12.0f, 12.0f), false, 12.5f, 1);
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        AttackMaker((int)(attack * 14.5f), 3, attentionDamage, attentionRate, this.transform.position, new Vector2(5.5f, 5.5f), 800, false);
        mainCameraControllerScript.ShakeCamera(0.25f, 0.5f, 90, 15, false, true);
    }

    //�L����ID��7�̃L�����̕K�E�Z
    public void Char7SpecialMove()
    {
        
    }

    //�L����ID��8�̃L�����̕K�E�Z
    public void Char8SpecialMove()
    {
        
    }

    //�X�L��
    public void Skill()
    {
        if (currentSkillRecharge <= 0 && isHookShotMoving == false)  
        {
            currentSkillRecharge = maxSkillRecharge;

            if (charId == 1)
            {
                Char1Skill();
            }
            else if (charId == 2)
            {
                Char2Skill();
            }
            else if (charId == 3)
            {
                Char3Skill();
            }
            else if (charId == 4)
            {
                Char4Skill();
            }
            else if (charId == 5)
            {
                Char5Skill();
            }
            else if (charId == 6)
            {
                Char6Skill();
            }
            else if (charId == 7)
            {
                Char7Skill();
            }
            else if (charId == 8)
            {
                Char8Skill();
            }
        }
    }

    //�L����ID��1�̃L�����̃X�L��
    async public void Char1Skill()
    {
        //5.0�b�Ԓʏ�U�����G�[�e�������ɕω�(�o�O�Ή������邱��(�T���ɖ߂�����L�����Z��))
        normalAttackAttribute = 5;

        await UniTask.Delay(TimeSpan.FromSeconds(7.0f));

        //�ʏ�U���𕨗������ɖ߂�
        normalAttackAttribute = 0;
    }

    //�L����ID��2�̃L�����̃X�L��
    public void Char2Skill()
    {
        //HP����(�؂�グ)
        currentHp = (int)Mathf.Ceil((float)(currentHp) / 2);
        //���̍U�����m���S
        isChar2Attention = true;
    }

    //�L����ID��3�̃L�����̃X�L��
    public void Char3Skill()
    {
        
        //�W�G����
        Vacuum(this.transform.position, new Vector2(12.5f, 12.5f), 0.5f, 0.1f);
        //�U��
        AttackMaker((int)(attack * 4.5f), 3, attentionDamage, attentionRate, this.transform.position, new Vector2(10.0f, 10.0f), 0, false);
    }

    //�L����ID��4�̃L�����̃X�L��
    public void Char4Skill()
    {
        for(int i = 0; i < 9; i++) 
        {
            Arrow(i + 1, (int)(attack * 0.7f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(0.2f, 0.2f), new Vector2(1.0f, 1.0f), 10, 0);
        }
    }

    //�L����ID��5�̃L�����̃X�L��
    public void Char5Skill()
    {
        Vector2 vacuumPos = new Vector2(transform.position.x + GetFacingDirection(isLookRight) * 3.0f, transform.position.y);
        //�W�G����
        Vacuum(vacuumPos, new Vector2(7.5f, 7.5f), 5.0f, 0.1f);
    }

    //�L����ID��6�̃L�����̃X�L��
    async public void Char6Skill()
    {
        float healValue = maxHp * 0.05f;
        for(int i = 0; i < 6; i++) 
        {
            Heal(true, healValue);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        }
    }

    //�L����ID��7�̃L�����̃X�L��
    public void Char7Skill()
    {
        
    }

    //�L����ID��8�̃L�����̃X�L��
    async public void Char8Skill()
    {
        for (int i = 0; i < 10; i++) 
        {
            int randomAttribute = UnityEngine.Random.Range(0, 7);
            Arrow(1, (int)(attack * 1.2f), randomAttribute, attentionDamage, attentionRate, this.transform.position, new Vector2(0.2f, 0.2f), new Vector2(0, 0), 10, 0);
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
        }
    }

    //����
    private void Characteristic()
    {
        //�`�[���L�����̔c��
        int[] teamId = { teamCoutnrollerScript.TeamIdData[0], teamCoutnrollerScript.TeamIdData[1], teamCoutnrollerScript.TeamIdData[2] };

        if (charId == 1)
        {
            //Char1Characteristic();
        }
        if (charId == 2)
        {
            Char2Characteristic();
        }
        if (teamId.Contains(3)) //�T�����甭���\ 
        {
            Char3Characteristic();
        }
        if (teamId.Contains(4)) //�T�����甭���\
        {
            Char4Characteristic();
        }
        if (charId == 5)
        {
            Char5Characteristic();
        }
        if (charId == 6)
        {
            Char6Characteristic();
        }
        if (charId == 7)
        {
            Char7Characteristic();
        }
        if (charId == 8)
        {
            Char8Characteristic();
        }
    }

    //�L����ID��1�̃L�����̓���
    public void Char1Characteristic()
    {
        //�G��|�����Ƃ�HP��
        Heal(false, maxHp * 0.15f);
    }

    //�L����ID��2�̃L�����̓���
    public void Char2Characteristic()
    {
        //HP�������ȉ��̂Ƃ��U����2�{
        if (currentHp <= Mathf.Ceil((float)maxHp / 2)) 
        {
            //�_���[�W2�{
            damageBuff *= 2.0f;
        }
    }

    //�L����ID��3�̃L�����̓���
    public void Char3Characteristic()
    {
        //�_���[�W�y��
        DamageReduction(25.0f);
    }

    //�L����ID��4�̃L�����̓���
    public void Char4Characteristic()
    {
        //�T������ł��p�������邱��
        int tA1 = dB_charData.charData[teamCoutnrollerScript.TeamIdData[0]].attribute;
        int tA2 = dB_charData.charData[teamCoutnrollerScript.TeamIdData[1]].attribute;
        int tA3 = dB_charData.charData[teamCoutnrollerScript.TeamIdData[2]].attribute;
        //3�����قȂ�
        if (tA1 != tA2 && tA2 != tA3 && tA3 != tA1)
        {
            damageBuff *= 1.4f;
        }
        //2�����قȂ�
        else if ((tA1 == tA2 && tA2 == tA3 && tA3 == tA1) == false) 
        {
            damageBuff *= 1.2f;
        }
    }

    //�L����ID��5�̃L�����̓���
    public void Char5Characteristic()
    {
        //
    }

    //�L����ID��6�̃L�����̓���
    public void Char6Characteristic()
    {
        baseAttackBuff += Mathf.CeilToInt(currentHp * 0.025f);
    }

    //�L����ID��7�̃L�����̓���
    public void Char7Characteristic()
    {
        
    }

    //�L����ID��8�̃L�����̓���
    public void Char8Characteristic()
    {
        
    }

    //��_���[�W
    public void Damage(EnemyAction enemyAction, int damage, int enemyAttribute)
    {
        //���G�ł͂Ȃ��Ƃ�
        if (invincibilityTimer <= 0)
        {
            damage = GameSystemUtility.CalcDamage(damage, enemyAttribute, attribute, () => StartCoroutine(PairAnnihilationDamage(enemyAction, damage)));

            //�_���[�W�y��
            if (damageReductionRate != 0) 
            {
                if (damageReductionRate <= 100)
                {
                    damage = Mathf.CeilToInt((float)(damage) * (1 - (damageReductionRate / 100)));
                }
                else
                {
                    damage = 0;
                }
            }
            
            currentHp -= damage;

            //�_���[�W�\�L�Ăяo��
            var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
            DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
            //�����͍U���́A�����A��S����(��)�A�\�L�̔����ꏊ(�����̔�_���[�W�ɉ�S�_���[�W�͔������Ȃ�)
            damageNotationObjsScript.DamageNotion(damage, enemyAttribute, false, (Vector2)transform.position + new Vector2(0, 0.0f));

            //DamageNotationCountroller damageNotationObjScript = damageNotationObj.GetComponent<DamageNotationCountroller>();
            //damageNotationObjScript.DamageNotation(damage, (Vector2)transform.position + new Vector2(0, 1.0f));
            //Debug.Log("Damaged" + damage);
            //Debug.Log("CurrentHP" + currentHp);

            //���G���Ԃ̍X�V
            InvincibilityTimer(invincibilityCoolTime);
        }
    }

    //�_���[�W�y���v�Z
    public void DamageReduction(float damageReductionRateValue) 
    {
        damageReductionRate += damageReductionRateValue;
    }

    //�d��
    public void Stun() 
    { 
        
    } 

    public int GetFacingDirection(bool isR) 
    {
        int getFacingDirection;
        if (isR == true) 
        { 
            getFacingDirection = 1;
        }
        else
        {
            getFacingDirection = -1;
        }
        return getFacingDirection;
    }

    //�Ώ��Ń_���[�W
    IEnumerator PairAnnihilationDamage(EnemyAction enemyAction, int damage)
    {
        if (pairAnnihilationDamageTimer <= 0) 
        {
            Debug.Log("�Ώ��Ń_���[�W");
            if (enemyAction == true)
            {
                enemyAction.EnemyAttack();
            }
            pairAnnihilationDamageTimer = pairAnnihilationDamageCoolTime;
        }
        yield break;
    }

    //���G����
    public void InvincibilityTimer(float timer) 
    {
        if (invincibilityTimer <= timer) 
        {
            invincibilityTimer = timer;
        }
    }

    //�X�^�~�i��(���R)
    private void Stamina()
    {
        staminaRecoveryCounter++;

        if (currentStamina < maxStamina && staminaRecoveryCounter >= 60 / staminaRecoverySpeed)
        {
            currentStamina++;
            staminaRecoveryCounter = 0;
        }
    }

    //�X�^�~�i����
    private void ExhaustStamina(int exhaustStamina)
    {
        currentStamina -= exhaustStamina;
    }

    //�L�����N�^�[�`�F���W
    public void CharChange(int changeCharId) //���̕\�ɏo��L������ID
    {
        charId = changeCharId;

        //�����ɃX�e�[�^�X���̓ǂݍ��݂��L�q
        CharDbReference();
    }

    //�L���������f�[�^�x�[�X����Q��
    public void CharDbReference() 
    {
        attribute = dB_charData.charData[charId].attribute;                 //����
        baseHp = dB_charData.charData[charId].baseHp;                       //��bHP
        baseAttack = dB_charData.charData[charId].baseAttack;               //��b�U����
        maxSkillRecharge = dB_charData.charData[charId].maxSkillRecharge;   //�X�L���N�[���^�C��
        maxSpecialMoveRecharge = dB_charData.charData[charId].maxSpecialMoveRecharge;   //�K�E�Z�N�[���^�C��
    }

    //�v���C���[���S
    private void PlayerDeath()
    {
        Debug.Log("PlayerDeath");
    }

    //�G��|����
    public void EnemyKill() 
    {
        if (charId == 1)
        {
            Char1Characteristic();
        }
        Debug.Log("EnemyKill");
    }

    /*
    //�����t��
    public int CurrentHP
    {
        get { return currentHp; } //getter�̕���
        set
        {
            if (value >= 0)  //setter�̕����A100�𒴂����Ƃ��̂ݑ������
                currentHp = value;
        }
    }
    */

    //maxHp�Q�Ɨp(getset)
    public int MaxHp // �v���p�e�B
    {
        get { return maxHp; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { maxHp = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }
    //public int MaxHp => maxHp;

    //currentHp�Q�Ɨp(getset)
    public int CurrentHp // �v���p�e�B
    {
        get { return currentHp; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { currentHp = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }

    //maxMp�Q�Ɨp(getset)
    public int MaxMp // �v���p�e�B
    {
        get { return maxMp; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { maxMp = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }

    //currentMp�Q�Ɨp(getset)
    public int CurrentMp // �v���p�e�B
    {
        get { return currentMp; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { currentMp = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }

    //maxStamina�Q�Ɨp(getset)
    public int MaxStamina // �v���p�e�B
    {
        get { return maxStamina; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { maxStamina = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }

    //currentStamina�Q�Ɨp(getset)
    public int CurrentStamina // �v���p�e�B
    {
        get { return currentStamina; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { currentStamina = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }

    //currentSkillRecharge�Q�Ɨp(getset)
    public float CurrentSkillRecharge // �v���p�e�B
    {
        get { return currentSkillRecharge; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { currentSkillRecharge = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }
    
    //currentSpecialMoveRecharge�Q�Ɨp(getset)
    public float CurrentSpecialMoveRecharge // �v���p�e�B
    {
        get { return currentSpecialMoveRecharge; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { currentSpecialMoveRecharge = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }

    /*
    //�X�N���v�gA(�Q�Ɛ�)
    public int Life // �v���p�e�B
    {
        get { return life; }  // �ʏ̃Q�b�^�[�B�Ăяo��������score���Q�Ƃł���
        set { life = value; } // �ʏ̃Z�b�^�[�Bvalue �̓Z�b�g���鑤�̐����Ȃǂ𔽉f����
    }
    //�X�N���v�gB(�Q�Ƃ����)
    int life = ball.Life; // �Q�b�^�[�BScriptA�̕ϐ����擾����
    */

    //�L����ID�󂯓n��
    public int CharId => charId;
    //�U���͎󂯓n��
    public int Attack => attack;
    //��S���󂯓n��
    public float AttentionDamage => attentionDamage;
    //��S���󂯓n��
    public float AttentionRate => attentionRate;
    //�E�������Ă��邩�̔���̎󂯓n��
    public bool IsLookRight => isLookRight;
}
