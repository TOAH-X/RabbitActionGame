using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;                  //Rigidbody2D
    [SerializeField] float moveSpeed = 3.0f;                    //�ړ����x
    [SerializeField] float baseMoveSpeed = 3.0f;                //��b�ړ����x
    [SerializeField] float jumpForce = 12.0f;                   //�W�����v��
    [SerializeField] float dashForce = 5.0f;                    //�_�b�V����
    [SerializeField] float hookShotMoveSpeed = 0.2f;            //�t�b�N�V���b�g�̈ړ����x
    [SerializeField] LayerMask groundLayer;                     //Layer��Ground
    [SerializeField] GameObject hookShotObj;                    //�t�b�N�V���b�g�̃v���n�u
    [SerializeField] GameObject normalAttackRangeObj;           //�ʏ�U���͈͂̃I�u�W�F�N�g

    [SerializeField] GameObject staminaNotationObj;             //�X�^�~�i�Q�[�W�̃I�u�W�F�N�g

    [SerializeField] GameObject playerSummonObjects;            //�v���C���[�������ł���I�u�W�F�N�g

    [SerializeField] GameObject vacuumRangeObj;                 //�z�����ݔ͈͂̃I�u�W�F�N�g

    [SerializeField] GameObject playerArrowObj;                 //��(�Ȃǉ������p)�̃I�u�W�F�N�g
    
    [SerializeField] List<GameObject> hookShotObjList = new List<GameObject>();     //�t�b�N�V���b�g�̃v���n�u���X�g
    [SerializeField] int hookShotObjCount = 0;                  //�t�b�N�V���b�g�̃J�E���g

    [SerializeField] int staminaRecoverySpeed = 60;             //�X�^�~�i�񕜑��x(1�b(60�t���[��)�Ŋ�񕜂ł��邩)��1�t���[��1�񕜂����x�A�����傫���قǊɂ₩�ɉ�

    [SerializeField] GameObject hpGaugeObj;                     //HP�Q�[�W�̃I�u�W�F�N�g
    [SerializeField] GameObject mpGaugeObj;                     //MP�Q�[�W�̃I�u�W�F�N�g

    [SerializeField] TeamCoutnroller teamCoutnrollerScript;     //TeamCoutnroller�̃X�N���v�g

    [SerializeField] float invincibilityTimer = 0;              //���G���ԊǗ�
    [SerializeField] float dashTimer = 0;                       //�_�b�V�����ԊǗ�(3��ȏ�A���Ń_�b�V�������Ȃ�)

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

    [Header("�U���o�t")]
    [SerializeField] float attackBuff = 1.0f;                   //�U���o�t(��)
    [Header("HP�o�t")]
    [SerializeField] float hpBuff = 1.0f;                       //HP�o�t(��)
    [Header("�_���[�W�o�t")]
    [SerializeField] float damageBuff = 1.0f;                   //�_���[�W�o�t(��)

    //Update�֐����Ńo�t�̌v�Z�𖈃t���[���s������
    //�����o�t�������邱��
    private bool isLookRight = true;                            //��������(�E�A�O�������Ă��邩)
    private bool isHookShotMoving = false;                      //�t�b�N�V���b�g�ňړ�����

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

        rb2D = GetComponent<Rigidbody2D>();

        //�X�e�[�^�X���f�[�^�x�[�X����Q��
        CharDbReference();

        //�X�e�[�^�X���
        currentHp = maxHp;
        currentMp = maxMp;
        currentStamina = maxStamina;

        //Canvas��������(�_���[�W�\���p�Ȃ�)
        canvasObj = GameObject.Find("Canvas");
        canvasTransform = canvasObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //FPS�v��
        //float fps = 1f / Time.deltaTime;
        //Debug.Log("FPS:" + fps);

        //�U���͌v�Z
        attack = (int)Mathf.Ceil((float)((baseAttack) * attackBuff) * damageBuff);
        //HP�v�Z
        maxHp = (int)Mathf.Ceil((float)(baseHp) * hpBuff);
        //�U���̓o�t�̃��t���b�V��
        attackBuff = 1.0f;
        //HP�o�t�̃��t���b�V��
        hpBuff = 1.0f;
        //�_���[�W�o�t�̃��t���b�V��
        damageBuff = 1.0f;

        //����ړ�(�t�b�N�V���b�g)���ł͂Ȃ��Ƃ�
        if (isHookShotMoving == false)
        {
            //�ړ�
            MoveUpdate();

            //����ړ�(�t�b�N�V���b�g)...�ꕔ�L�����̓���ړ��H�X�L���ɂ���\��
            HookShot();

            //�ʏ�U��
            NormalAttack(attack, normalAttackAttribute);           //�U���͈ˑ�

            //Ray�̔���(�n��ɂ���Ƃ�)
            if (IsGrounding() == true)
            {
                //�W�����v
                JumpUpdate();

                //�_�b�V��
                Dash();
            }
        }

        //����
        Characteristic();

        //�K�E�Z
        if (Input.GetKeyDown(KeyCode.Q))
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
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World); // �ړ������ɉ����Ĉړ�
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
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    //�n�ʂƂ̐ڒn����
    private bool IsGrounding()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundLayer);
        //Debug.DrawRay(transform.position, Vector2.down * 0.6f, Color.red);
        float distance = 1.1f;  // BoxCast�̋���
        Vector2 boxSize = new Vector2(0.5f, 0.5f);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, transform.localEulerAngles.z, Vector2.down, distance, groundLayer);

        // BoxCast�͈̔͂�`�� (�l�p�`�̕`��)
        Vector3 topLeft = transform.position + new Vector3(-boxSize.x / 2, boxSize.y / 2, 0);
        Vector3 topRight = transform.position + new Vector3(boxSize.x / 2, boxSize.y / 2, 0);
        Vector3 bottomLeft = transform.position + new Vector3(-boxSize.x / 2, -boxSize.y / 2, 0);
        Vector3 bottomRight = transform.position + new Vector3(boxSize.x / 2, -boxSize.y / 2, 0);

        // �{�b�N�X�̏㉺�ӂ�`��
        Debug.DrawLine(topLeft, topLeft + Vector3.down * distance, Color.red);  // ������
        Debug.DrawLine(topRight, topRight + Vector3.down * distance, Color.red);  // �E����
        Debug.DrawLine(bottomLeft, bottomLeft + Vector3.down * distance, Color.red);  // ���� (��)
        Debug.DrawLine(bottomRight, bottomRight + Vector3.down * distance, Color.red);  // ���� (�E)

        return hit.collider != null;
    }

    //�_�b�V��
    private void Dash()
    {
        if (Input.GetMouseButtonDown(1) && currentStamina > 10 && dashTimer <= 0)
        {
            //�X�^�~�i����
            ExhaustStamina(10);
            //�_�b�V���\���Ԃ̍X�V
            dashTimer = 1;

            if (isLookRight == true)
            {
                rb2D.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
            }
            else
            {
                rb2D.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
            }
        }
    }

    //����ړ�(�t�b�N�V���b�g)�̏���
    private void HookShot()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.R) && currentStamina > 20)
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
            //Instantiate(hookShotObj,touchScreenPos,Quaternion.identity);

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
    //�L����ID�A�{���v�Z��̍U���́A�����A��S�_���[�W�A��S���A�U�������ꏊ�A�U���͈͂̑傫��(x,y)�A�m�b�N�o�b�N��(charID�������Ă���̂ŉ����邱��)
    public void AttackMaker(int multipliedAttack, int finalAttributeNormalAttack, float multipliedAttentionDamage, float multipliedAttentionRate, Vector3 attackPos, Vector2 attackSize, float knockBackValue)
    {
        //char2���m���S�̂Ƃ�
        if (isChar2Attention == true)
        {
            multipliedAttentionRate = 100;
            isChar2Attention = false;
        }

        var normalAttackRangeObjs = Instantiate(normalAttackRangeObj, attackPos, this.transform.rotation);
        normalAttackRangeObjs.transform.localScale = new Vector3(attackSize.x, attackSize.y, 1.0f);
        NormalAttackRange normalAttackRangeObjsScript = normalAttackRangeObjs.GetComponent<NormalAttackRange>();
        normalAttackRangeObjsScript.NormalAttack(charId, multipliedAttack, finalAttributeNormalAttack, multipliedAttentionDamage, multipliedAttentionRate, knockBackValue);
    }

    //��
    public void Heal(float healValue) 
    {
        if (currentHp + (int)Mathf.Ceil(healValue) >= maxHp)  
        {
            currentHp = maxHp;
        }
        else 
        {
            currentHp += (int)Mathf.Ceil(healValue);
        }
        //�_���[�W�\�L�Ăяo��
        var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
        DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
        //�����͍U���́A�����A��S����(��)�A�\�L�̔����ꏊ(�����̔�_���[�W�ɉ�S�_���[�W�͔������Ȃ�)
        damageNotationObjsScript.DamageNotion((int)Mathf.Ceil(healValue), -1, false, (Vector2)transform.position + new Vector2(0, 0.0f));
    }

    //�W�G(�ŗL�X�L���ȂǂȂ̂Ń��t�@�N�^�����O�ł͕ʃX�N���v�g�ɂ��Ă�������)
    //�W�G�����ꏊ�A�W�G�͈́A�W�G��������(�b)
    public void Vacuum(Vector2 vacuumPos, Vector2 vacuumSize, float vacuumDuration) 
    {
        //vacuumPos = this.transform.position;                //�W�G���W
        //vacuumSize = new Vector2(10.0f, 10.0f);             //�W�G�͈�
        //vacuumDuration = 0.5f;                              //�W�G�̎�������(�b)
        //�z������
        var vacuumRangeObjs = Instantiate(vacuumRangeObj, vacuumPos, this.transform.rotation);
        vacuumRangeObjs.transform.localScale = new Vector3(vacuumSize.x, vacuumSize.y, 1.0f);
        VacuumRange vacuumRangeObjsScript = vacuumRangeObjs.GetComponent<VacuumRange>();
        vacuumRangeObjsScript.Vacuum(vacuumDuration);
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
    //�U���^�C�v(�ʏ�0)�A�p������(summonDuration)�A�m�b�N�o�b�N�ʁA�����ʒu�A�T�C�Y
    public void Arrow(int attackType, int multipliedAttack, int arrowAttribute, float arrowAttentionDamage, float arrowAttentionRate, Vector2 playerArrowObjsPos, Vector2 playerArrowObjsSize, float arrowKnockBackValue)
    {
        //�ݒu
        var playerArrowObjs = Instantiate(playerArrowObj, playerArrowObjsPos, this.transform.rotation);
        playerArrowObjs.transform.localScale = new Vector3(playerArrowObjsSize.x, playerArrowObjsSize.y, 1.0f);
        PlayerArrowObject playerArrowObjectScript = playerArrowObjs.GetComponent<PlayerArrowObject>();
        playerArrowObjectScript.Arrow(GetComponent<Player>(), charId, attackType, multipliedAttack, arrowAttribute, arrowAttentionDamage, arrowAttentionRate, arrowKnockBackValue);
    }
    

    //�ʏ�U��(�U����,����)
    private void NormalAttack(int attack, int attributeNormalAttack)
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.E) == false)
        {
            //�E�����ƍ������Ŕ����ʒu��ύX
            float normalAttackDirection = 0.5f;
            if (isLookRight == false) 
            { 
                normalAttackDirection = -0.5f;
            }
            Vector3 normalAttackPos = this.transform.position;
            normalAttackPos.x += normalAttackDirection;

            AttackMaker(attack, attributeNormalAttack, attentionDamage, attentionRate, normalAttackPos, new Vector2(2.0f, 2.0f), 100);
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
                StartCoroutine(Char1SpecialMove());
            }
            else if (charId == 2)
            {
                StartCoroutine(Char2SpecialMove());
            }
            else if (charId == 3)
            {
                StartCoroutine(Char3SpecialMove());
            }
            else if (charId == 4)
            {
                StartCoroutine(Char4SpecialMove());
            }
            else if (charId == 5)
            {
                StartCoroutine(Char5SpecialMove());
            }
            else if (charId == 6)
            {
                StartCoroutine(Char6SpecialMove());
            }
        }
    }

    //�L����ID��1�̃L�����̕K�E�Z(timedeltatime���g������)
    IEnumerator Char1SpecialMove()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i <= 1)
            {
                AttackMaker((int)(attack * 2.4f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(12.5f, 12.5f), 120);

                for (int j = 0; j < 5; j++)
                {
                    yield return null;
                }
            }
            else
            {
                AttackMaker((int)(attack * 0.8f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(12.5f, 12.5f), 40);
            }

            for (int j = 0; j < 5; j++)
            {
                yield return null;
            }
        }
        yield break;
    }

    //�L����ID��2�̃L�����̕K�E�Z(timedeltatime���g������)
    IEnumerator Char2SpecialMove()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i == 0)
            {
                AttackMaker((int)(attack * 12.5f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(7.5f, 7.5f), 1200);

                for (int j = 0; j < 5; j++)
                {
                    yield return null;
                }
            }
            else
            {
                AttackMaker((int)(attack * 0.2f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(15.0f, 7.5f), 10);
            }

            for (int j = 0; j < 20; j++)
            {
                yield return null;
            }
        }

        yield break;
    }

    //�L����ID��3�̃L�����̕K�E�Z
    IEnumerator Char3SpecialMove()
    {
        PlayerSummonObjects(10, 0, this.transform.position, new Vector2(0.5f, 0.5f));

        yield break;
    }

    //�L����ID��4�̃L�����̕K�E�Z
    IEnumerator Char4SpecialMove()
    {
        PlayerSummonObjects(10, 0, this.transform.position, new Vector2(15.0f, 15.0f));

        yield break;
    }

    //�L����ID��5�̃L�����̕K�E�Z
    IEnumerator Char5SpecialMove()
    {
        
        yield break;
    }

    //�L����ID��6�̃L�����̕K�E�Z
    IEnumerator Char6SpecialMove()
    {

        yield break;
    }

    //�X�L��
    public void Skill()
    {
        if (currentSkillRecharge <= 0 && isHookShotMoving == false)  
        {
            currentSkillRecharge = maxSkillRecharge;

            if (charId == 1)
            {
                StartCoroutine(Char1Skill());
            }
            else if (charId == 2)
            {
                StartCoroutine(Char2Skill());
            }
            else if (charId == 3)
            {
                StartCoroutine(Char3Skill());
            }
            else if (charId == 4)
            {
                StartCoroutine(Char4Skill());
            }
            else if (charId == 5)
            {
                StartCoroutine(Char5Skill());
            }
            else if (charId == 6)
            {
                StartCoroutine(Char6Skill());
            }
        }
    }

    //�L����ID��1�̃L�����̃X�L��
    IEnumerator Char1Skill()
    {
        //5.0�b�Ԓʏ�U�����G�[�e�������ɕω�
        normalAttackAttribute = 5;

        float timer = 0;
        //����ł͓o��L�����ύX���Ƀo�O����������̂ŏC�����邱��(�L�����ύX���ɑ����ύX������������������)
        while (timer <= 7.0)  
        {
            timer += Time.deltaTime;
            yield return null;
        }

        //�ʏ�U���𕨗������ɖ߂�
        normalAttackAttribute = 0;

        yield break;
    }

    //�L����ID��2�̃L�����̃X�L��
    IEnumerator Char2Skill()
    {
        //HP����(�؂�グ)
        currentHp = (int)Mathf.Ceil((float)(currentHp) / 2);
        //���̍U�����m���S
        isChar2Attention = true;

        yield break;
    }

    //�L����ID��3�̃L�����̃X�L��
    IEnumerator Char3Skill()
    {
        //�W�G����
        Vacuum(this.transform.position, new Vector2(10, 10), 0.5f);
        //�U��
        AttackMaker((int)(attack * 4.5f), 3, attentionDamage, attentionRate, this.transform.position, new Vector2(10.0f, 10.0f), 0);

        yield break;
    }

    //�L����ID��4�̃L�����̃X�L��
    IEnumerator Char4Skill()
    {
        for(int i = 0; i < 9; i++) 
        {
            Arrow(i + 1, (int)(attack * 0.7f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(0.2f, 0.2f), 10);
        }
        
        yield break;
    }

    //�L����ID��5�̃L�����̃X�L��
    IEnumerator Char5Skill()
    {
        
        yield break;
    }

    //�L����ID��6�̃L�����̃X�L��
    IEnumerator Char6Skill()
    {

        yield break;
    }

    //����
    private void Characteristic()
    {
        //�`�[���L�����̔c��
        int[] teamId = { teamCoutnrollerScript.TeamIdData[0], teamCoutnrollerScript.TeamIdData[1], teamCoutnrollerScript.TeamIdData[2] };

        if (charId == 1)
        {
            //StartCoroutine(Char1Characteristic());
        }
        else if (charId == 2)
        {
            StartCoroutine(Char2Characteristic());
        }
        else if (charId == 3)
        {
            StartCoroutine(Char3Characteristic());
        }
        if (teamId.Contains(4))  //�T�����甭���\
        {
            StartCoroutine(Char4Characteristic());
        }
        else if (charId == 5)
        {
            StartCoroutine(Char5Characteristic());
        }
        else if (charId == 6)
        {
            StartCoroutine(Char6Characteristic());
        }
    }

    //�L����ID��1�̃L�����̓���
    IEnumerator Char1Characteristic()
    {
        //�G��|�����Ƃ�HP��
        Heal(maxHp * 0.15f);

        yield break;
    }

    //�L����ID��2�̃L�����̓���
    IEnumerator Char2Characteristic()
    {
        //HP�������ȉ��̂Ƃ��U����2�{
        if (currentHp <= Mathf.Ceil((float)maxHp / 2)) 
        {
            //�_���[�W2�{
            damageBuff *= 2.0f;
        }

        yield break;
    }

    //�L����ID��3�̃L�����̓���
    IEnumerator Char3Characteristic()
    {
        

        yield break;
    }

    //�L����ID��4�̃L�����̓���
    IEnumerator Char4Characteristic()
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

        yield break;
    }

    //�L����ID��5�̃L�����̓���
    IEnumerator Char5Characteristic()
    {


        yield break;
    }

    //�L����ID��6�̃L�����̓���
    IEnumerator Char6Characteristic()
    {


        yield break;
    }

    //��_���[�W
    public void Damage(int damage, int enemyAttribute)
    {
        //���G�ł͂Ȃ��Ƃ�
        if (invincibilityTimer <= 0)
        {
            currentHp -= damage;

            //�_���[�W�\�L�Ăяo��
            var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
            DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
            //�����͍U���́A�����A��S����(��)�A�\�L�̔����ꏊ(�����̔�_���[�W�ɉ�S�_���[�W�͔������Ȃ�)
            damageNotationObjsScript.DamageNotion(damage, enemyAttribute, false, (Vector2)transform.position + new Vector2(0, 0.0f));

            //DamageNotationCountroller damageNotationObjScript = damageNotationObj.GetComponent<DamageNotationCountroller>();
            //damageNotationObjScript.DamageNotation(damage, (Vector2)transform.position + new Vector2(0, 1.0f));
            //Debug.Log("Damaged" + damage);
            Debug.Log("CurrentHP" + currentHp);

            //���G���Ԃ̍X�V
            invincibilityTimer = 0.5f;
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
            Debug.Log(currentStamina);
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
            StartCoroutine(Char1Characteristic());
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
}
