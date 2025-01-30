using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEditor;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening;

public class EnemyAction : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2D;                          //�G���g��RigidBody2D
    [SerializeField] bool isDetectionPlayer = false;            //�v���C���[�𔭌����Ă��邩

    [SerializeField] int enemyId = 0;                           //�G��ID
    
    [SerializeField] int enemyAttack = 100;                     //�G�̍U����(���Data����Q�Ƃ��邱��)
    [SerializeField] int enemyAttribute = 1;                    //�G�̑���(���Data����Q�Ƃ��邱��)

    [SerializeField] int enemyLevel = 99;                       //�G�̃��x��

    [SerializeField] Sprite enemyPicture;                       //�G�̉摜

    [SerializeField] GameObject enemyAttackRangeObj;            //�G�̍U���͈͂̃I�u�W�F�N�g

    [SerializeField] Coroutine enemyDetectionPlayerCoroutine;   //�v���C���[�𔭌�������̋����̃R���[�`��

    [SerializeField] float enemyMoveSpeed = 0.25f;              //�G�̈ړ����x����ق�EnemyData����Q�Ƃ��邱��
    [SerializeField] float jumpForce = 20.0f;                   //�W�����v��

    [SerializeField] GameObject enemyDropItemObj;               //�h���b�v�A�C�e���̃I�u�W�F�N�g(�v���n�u)

    private bool isEnemyLookRight = true;                       //��������(�E�A�O�������Ă��邩)

    [SerializeField] LayerMask groundLayer;                     //Layer��Ground

    private GameObject playerObj;                               //�v���C���[�I�u�W�F�N�g(Ray�ł�Player�^�O�A���̑��ł͂���Obj��Player�𔻕ʂ��Ă��ĕʁX�ɂȂ��Ă���̂ŗ]�T������Β����Ă���)

    [SerializeField] const float knockBackStrength = 10.0f;     //�m�b�N�o�b�N�̔򋗗�

    [SerializeField] GameObject canvasObj;                      //�L�����o�X�I�u�W�F�N�g
    [SerializeField] Transform canvasTransform;                 //�L�����o�X�̈ʒu���
    [SerializeField] EnemyLevelNotation enemyLevelNotationPrefab;   //�G�̃��x���\�L�e�L�X�g�I�u�W�F�N�g
    private EnemyLevelNotation enemyLevelNotation;                  //�G�̃��x���\�L�v���n�u�̃X�N���v�g
    [SerializeField] GameObject enemyHpNotationObj;        //�G��HP�\�L�e�L�X�g�I�u�W�F�N�g
    private GameObject enemyHpNotationPrefab;                     //�G�̃��x���\�L�v���n�u�̃X�N���v�g

    private Enemy enemyScript;                                  //Enemy�X�N���v�g

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        //(�]�͂�����Όy�������ɏ��������邱��)
        playerObj = GameObject.Find("Player");

        //�X�e�[�^�X�Q��
        enemyScript = this.gameObject.GetComponent<Enemy>();
        enemyId = enemyScript.EnemyId;
        enemyAttribute = enemyScript.EnemyAttribute;
        enemyAttack = enemyScript.EnemyAttack;
        enemyLevel = enemyScript.EnemyLevel;
        enemyPicture = enemyScript.EnemyPicture;

        //�摜��ύX(�{���͐F�̕ύX�ł͂Ȃ����������)
        //SpriteRenderer thisSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        //thisSpriteRenderer.sprite = enemyPicture;
        if (enemyId == 1) 
        {
            //this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(240, 10, 10, 255);

        }

        //�_���S���`����o�ꃂ�[�V����(�h��)
        if (enemyId >= 1 && enemyId <= 6) 
        {
            transform.DOComplete();
            transform.DOShakeScale(1f, 1f, 30, 90f, true);      //���ԁA�U���̋����A�U�����A�����_���x�A�t�F�[�h�A�E�g���邩
        }


        //���x���\�L
        //(�]�͂�����Όy�������ɏ��������邱��)
        canvasObj = GameObject.Find("EnemyNotationCanvas");
        canvasTransform = canvasObj.transform;
        //���x���\�L�Ăяo��
        enemyLevelNotation = Instantiate<EnemyLevelNotation>(enemyLevelNotationPrefab, transform.position, Quaternion.identity, canvasTransform);
        enemyLevelNotation.LevelNotation(enemyLevel);

        //HP�\�L
        //(�]�͂�����Όy�������ɏ��������邱��)
        //canvasObj = GameObject.Find("EnemyNotationCanvas");
        //canvasTransform = canvasObj.transform;
        //HP�\�L�Ăяo��
        enemyHpNotationPrefab = Instantiate(enemyHpNotationObj, transform.position, Quaternion.identity, canvasTransform);
        //enemyHpNotation.LevelNotation(enemyLevel);


        //���V�p�A�ŏ�����v���C���[�𔭌�
        LookPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //ray���΂�(true�Ȃ�Player����)
        if (enemyDetectionPlayerCoroutine == null)
        {
            isDetectionPlayer = EnemyRayForward();
        }

        //�v���C���[������̋���
        if (isDetectionPlayer == true && enemyDetectionPlayerCoroutine == null)
        {
            enemyDetectionPlayerCoroutine = StartCoroutine(EnemyDetectionAction());
        }

        //���x���\�L�̒Ǐ]
        enemyLevelNotation.transform.position = this.transform.position + new Vector3(0, 1.0f, 0);
        //HP�\�L�̒Ǐ]
        enemyHpNotationPrefab.transform.position = this.transform.position + new Vector3(0, 0.75f, 0);
    }

    //�G�̈ړ�
    IEnumerator EnemyMove()
    {
        //�e�X�g�p����
        /*
        Vector3 scale = transform.localScale;
        if (isEnemyLookRight == true) 
        {
            scale.x = 1;
            rb2D.velocity = new Vector2(enemyMoveSpeed, 0);
        }
        else 
        {
            scale.x = -1;
            rb2D.velocity = new Vector2(-enemyMoveSpeed, 0);
        }
        */

        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }

        //�_���S���`�̋���(�W�����v)
        //transform.DOComplete();
        //transform.DOScaleX(0.5f, 1f);
        if (isEnemyLookRight == true)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            for (int i = 0; i < 10; i++)
            {
                rb2D.velocity = new Vector2(enemyMoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
        }
        else
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            for (int i = 0; i < 10; i++)
            {
                rb2D.velocity = new Vector2(-enemyMoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
        }

        yield break;
    }

    //�G�̍U��
    public void EnemyAttack()
    {
        //�G�̍U��
        var enemyAttackRangeObjs = Instantiate(enemyAttackRangeObj, this.transform.position, this.transform.rotation);
        enemyAttackRangeObjs.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        EnemyAttackRange enemyAttackRangeObjsScript = enemyAttackRangeObjs.GetComponent<EnemyAttackRange>();
        enemyAttackRangeObjsScript.EnemyAttack(this, enemyAttack, enemyAttribute);
    }

    //����(Ray���΂�)
    public bool EnemyRayForward()
    {
        //Ray
        Vector2 origin = this.transform.position;
        Vector2 direction = new Vector2(1.0f, 0);
        //�������̎��͍��ɕύX
        if (isEnemyLookRight == false)
        {
            direction = new Vector2(-1, 0);
        }
        //Debug.Log(direction);
        float distance = 5.0f;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        if (isEnemyLookRight == false)
        {
            Array.Sort(hits, (x, y) => x.distance.CompareTo(-y.distance));
        }

        //Debug.DrawRay(origin, direction * distance, Color.red);

        foreach (var hit in hits)
        {
            if (hit.collider != this.gameObject)
            {
                //Debug.Log("Hit object: " + hit.collider.name + ", Tag: " + hit.collider.tag);
                if (hit.collider.tag == "Player")
                {
                    //Debug.Log("hit");
                    //�v���C���[�����������ꍇ
                    return true;
                }
            }
        }
        //������Ȃ������ꍇ
        return false;
    }

    //�v���C���[�̕���������
    public void LookPlayer() 
    {
        if (this.transform.position.x <= playerObj.transform.position.x) 
        {
            isEnemyLookRight = true;
        }
        else 
        { 
            isEnemyLookRight = false;
        }
    }

    //�G�̎��S����
    public void EnemyDeath(int latestAttackCharId)
    {
        //���g��|�����v���C���[�ɑ΂��鏈��
        //GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) 
        {
            Player playerScript = playerObj.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.EnemyKill();
            }
        }

        //�A�C�e���h���b�v
        if (enemyDropItemObj != null) 
        {
            //GameObject enemyDropItemObjs = Instantiate(enemyDropItemObj, this.transform.position, this.transform.rotation);
            //Rigidbody2D enemyDropItemObjsrb2D = enemyDropItemObjs.GetComponent<Rigidbody2D>();
            //enemyDropItemObjsrb2D.AddForce(new Vector2(0, 5.0f), ForceMode2D.Impulse);
        }
        //����
        DOTween.Kill(transform);
        Destroy(enemyLevelNotation.gameObject);
        Destroy(enemyHpNotationPrefab);
        Destroy(gameObject);
    }


    //�v���C���[�������̋���
    IEnumerator EnemyDetectionAction() 
    {
        while (isDetectionPlayer == true) 
        {
            Coroutine enemyMoveCoroutine = null;

            //�W�����v�ړ�
            if (enemyMoveCoroutine == null && IsGrounding() == true)
            {
                LookPlayer();
                enemyMoveCoroutine = StartCoroutine(EnemyMove());
            }

            //���n����܂ő҂�
            while (IsGrounding() == false) 
            {
                yield return null;
            }

            //�U��
            EnemyAttack();

            //isDetectionPlayer = false;

            int waitTimer = UnityEngine.Random.Range(30, 60);
            for (int i = 0; i < waitTimer; i++)
            {
                yield return null;
            }

            //�����̑���(������x����Ă����猩���������)
            if (Vector2.Distance((Vector2)(this.transform.position), (Vector2)(playerObj.transform.position)) >= 10) 
            {
                isDetectionPlayer = false;
                enemyDetectionPlayerCoroutine = null;
                yield break;
            }

            yield return null;

        }

        yield break;
    }

    //�m�b�N�o�b�N(�U����s���Ȃǂ����f�����邱��)
    public void EnemyKnockBack(bool isKnockBackRight) 
    {
        //�E�ɐ������
        if (isKnockBackRight == true) 
        {
            rb2D.AddForce(new Vector2(knockBackStrength, knockBackStrength), ForceMode2D.Impulse);
        }
        //���ɐ������
        else
        {
            rb2D.AddForce(new Vector2(-knockBackStrength, knockBackStrength), ForceMode2D.Impulse);
        }
        //�_���S���`����o�ꃂ�[�V����(�h��)
        if (enemyId >= 1 && enemyId <= 6)
        {
            transform.DOComplete();
            transform.DOShakeScale(1f, 1f, 30, 90f, true);      //���ԁA�U���̋����A�U�����A�����_���x�A�t�F�[�h�A�E�g���邩
        }
    }

    //�n�ʂƂ̐ڒn����
    private bool IsGrounding()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundLayer);
        //Debug.DrawRay(transform.position, Vector2.down * 0.6f, Color.red);
        float distance = 0.5f;  // BoxCast�̋���
        Vector2 boxSize = new Vector2(1.0f, 1.0f);
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
}
