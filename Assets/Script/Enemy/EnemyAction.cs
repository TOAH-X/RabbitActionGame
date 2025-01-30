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
    [SerializeField] Rigidbody2D rb2D;                          //敵自身のRigidBody2D
    [SerializeField] bool isDetectionPlayer = false;            //プレイヤーを発見しているか

    [SerializeField] int enemyId = 0;                           //敵のID
    
    [SerializeField] int enemyAttack = 100;                     //敵の攻撃力(後でDataから参照すること)
    [SerializeField] int enemyAttribute = 1;                    //敵の属性(後でDataから参照すること)

    [SerializeField] int enemyLevel = 99;                       //敵のレベル

    [SerializeField] Sprite enemyPicture;                       //敵の画像

    [SerializeField] GameObject enemyAttackRangeObj;            //敵の攻撃範囲のオブジェクト

    [SerializeField] Coroutine enemyDetectionPlayerCoroutine;   //プレイヤーを発見した後の挙動のコルーチン

    [SerializeField] float enemyMoveSpeed = 0.25f;              //敵の移動速度※後ほどEnemyDataから参照すること
    [SerializeField] float jumpForce = 20.0f;                   //ジャンプ力

    [SerializeField] GameObject enemyDropItemObj;               //ドロップアイテムのオブジェクト(プレハブ)

    private bool isEnemyLookRight = true;                       //向き判定(右、前を向いているか)

    [SerializeField] LayerMask groundLayer;                     //LayerのGround

    private GameObject playerObj;                               //プレイヤーオブジェクト(RayではPlayerタグ、その他ではこのObjでPlayerを判別していて別々になっているので余裕があれば直しておく)

    [SerializeField] const float knockBackStrength = 10.0f;     //ノックバックの飛距離

    [SerializeField] GameObject canvasObj;                      //キャンバスオブジェクト
    [SerializeField] Transform canvasTransform;                 //キャンバスの位置情報
    [SerializeField] EnemyLevelNotation enemyLevelNotationPrefab;   //敵のレベル表記テキストオブジェクト
    private EnemyLevelNotation enemyLevelNotation;                  //敵のレベル表記プレハブのスクリプト
    [SerializeField] GameObject enemyHpNotationObj;        //敵のHP表記テキストオブジェクト
    private GameObject enemyHpNotationPrefab;                     //敵のレベル表記プレハブのスクリプト

    private Enemy enemyScript;                                  //Enemyスクリプト

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        //(余力があれば軽い処理に書き換えること)
        playerObj = GameObject.Find("Player");

        //ステータス参照
        enemyScript = this.gameObject.GetComponent<Enemy>();
        enemyId = enemyScript.EnemyId;
        enemyAttribute = enemyScript.EnemyAttribute;
        enemyAttack = enemyScript.EnemyAttack;
        enemyLevel = enemyScript.EnemyLevel;
        enemyPicture = enemyScript.EnemyPicture;

        //画像を変更(本来は色の変更ではなくこれをする)
        //SpriteRenderer thisSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        //thisSpriteRenderer.sprite = enemyPicture;
        if (enemyId == 1) 
        {
            //this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(240, 10, 10, 255);

        }

        //ダンゴモチ限定登場モーション(揺れ)
        if (enemyId >= 1 && enemyId <= 6) 
        {
            transform.DOComplete();
            transform.DOShakeScale(1f, 1f, 30, 90f, true);      //時間、振動の強さ、振動数、ランダム度、フェードアウトするか
        }


        //レベル表記
        //(余力があれば軽い処理に書き換えること)
        canvasObj = GameObject.Find("EnemyNotationCanvas");
        canvasTransform = canvasObj.transform;
        //レベル表記呼び出し
        enemyLevelNotation = Instantiate<EnemyLevelNotation>(enemyLevelNotationPrefab, transform.position, Quaternion.identity, canvasTransform);
        enemyLevelNotation.LevelNotation(enemyLevel);

        //HP表記
        //(余力があれば軽い処理に書き換えること)
        //canvasObj = GameObject.Find("EnemyNotationCanvas");
        //canvasTransform = canvasObj.transform;
        //HP表記呼び出し
        enemyHpNotationPrefab = Instantiate(enemyHpNotationObj, transform.position, Quaternion.identity, canvasTransform);
        //enemyHpNotation.LevelNotation(enemyLevel);


        //試遊用、最初からプレイヤーを発見
        LookPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //rayを飛ばす(trueならPlayer発見)
        if (enemyDetectionPlayerCoroutine == null)
        {
            isDetectionPlayer = EnemyRayForward();
        }

        //プレイヤー発見後の挙動
        if (isDetectionPlayer == true && enemyDetectionPlayerCoroutine == null)
        {
            enemyDetectionPlayerCoroutine = StartCoroutine(EnemyDetectionAction());
        }

        //レベル表記の追従
        enemyLevelNotation.transform.position = this.transform.position + new Vector3(0, 1.0f, 0);
        //HP表記の追従
        enemyHpNotationPrefab.transform.position = this.transform.position + new Vector3(0, 0.75f, 0);
    }

    //敵の移動
    IEnumerator EnemyMove()
    {
        //テスト用挙動
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

        //ダンゴモチの挙動(ジャンプ)
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

    //敵の攻撃
    public void EnemyAttack()
    {
        //敵の攻撃
        var enemyAttackRangeObjs = Instantiate(enemyAttackRangeObj, this.transform.position, this.transform.rotation);
        enemyAttackRangeObjs.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        EnemyAttackRange enemyAttackRangeObjsScript = enemyAttackRangeObjs.GetComponent<EnemyAttackRange>();
        enemyAttackRangeObjsScript.EnemyAttack(this, enemyAttack, enemyAttribute);
    }

    //視線(Rayを飛ばす)
    public bool EnemyRayForward()
    {
        //Ray
        Vector2 origin = this.transform.position;
        Vector2 direction = new Vector2(1.0f, 0);
        //左向きの時は左に変更
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
                    //プレイヤーが見つかった場合
                    return true;
                }
            }
        }
        //見つからなかった場合
        return false;
    }

    //プレイヤーの方向を見る
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

    //敵の死亡処理
    public void EnemyDeath(int latestAttackCharId)
    {
        //自身を倒したプレイヤーに対する処理
        //GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) 
        {
            Player playerScript = playerObj.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.EnemyKill();
            }
        }

        //アイテムドロップ
        if (enemyDropItemObj != null) 
        {
            //GameObject enemyDropItemObjs = Instantiate(enemyDropItemObj, this.transform.position, this.transform.rotation);
            //Rigidbody2D enemyDropItemObjsrb2D = enemyDropItemObjs.GetComponent<Rigidbody2D>();
            //enemyDropItemObjsrb2D.AddForce(new Vector2(0, 5.0f), ForceMode2D.Impulse);
        }
        //消滅
        DOTween.Kill(transform);
        Destroy(enemyLevelNotation.gameObject);
        Destroy(enemyHpNotationPrefab);
        Destroy(gameObject);
    }


    //プレイヤー発見中の挙動
    IEnumerator EnemyDetectionAction() 
    {
        while (isDetectionPlayer == true) 
        {
            Coroutine enemyMoveCoroutine = null;

            //ジャンプ移動
            if (enemyMoveCoroutine == null && IsGrounding() == true)
            {
                LookPlayer();
                enemyMoveCoroutine = StartCoroutine(EnemyMove());
            }

            //着地するまで待つ
            while (IsGrounding() == false) 
            {
                yield return null;
            }

            //攻撃
            EnemyAttack();

            //isDetectionPlayer = false;

            int waitTimer = UnityEngine.Random.Range(30, 60);
            for (int i = 0; i < waitTimer; i++)
            {
                yield return null;
            }

            //距離の測定(ある程度離れていたら見失う判定に)
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

    //ノックバック(攻撃や行動なども中断させること)
    public void EnemyKnockBack(bool isKnockBackRight) 
    {
        //右に吹き飛ぶ
        if (isKnockBackRight == true) 
        {
            rb2D.AddForce(new Vector2(knockBackStrength, knockBackStrength), ForceMode2D.Impulse);
        }
        //左に吹き飛ぶ
        else
        {
            rb2D.AddForce(new Vector2(-knockBackStrength, knockBackStrength), ForceMode2D.Impulse);
        }
        //ダンゴモチ限定登場モーション(揺れ)
        if (enemyId >= 1 && enemyId <= 6)
        {
            transform.DOComplete();
            transform.DOShakeScale(1f, 1f, 30, 90f, true);      //時間、振動の強さ、振動数、ランダム度、フェードアウトするか
        }
    }

    //地面との接地判定
    private bool IsGrounding()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundLayer);
        //Debug.DrawRay(transform.position, Vector2.down * 0.6f, Color.red);
        float distance = 0.5f;  // BoxCastの距離
        Vector2 boxSize = new Vector2(1.0f, 1.0f);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, transform.localEulerAngles.z, Vector2.down, distance, groundLayer);

        // BoxCastの範囲を描画 (四角形の描画)
        Vector3 topLeft = transform.position + new Vector3(-boxSize.x / 2, boxSize.y / 2, 0);
        Vector3 topRight = transform.position + new Vector3(boxSize.x / 2, boxSize.y / 2, 0);
        Vector3 bottomLeft = transform.position + new Vector3(-boxSize.x / 2, -boxSize.y / 2, 0);
        Vector3 bottomRight = transform.position + new Vector3(boxSize.x / 2, -boxSize.y / 2, 0);

        // ボックスの上下辺を描画
        Debug.DrawLine(topLeft, topLeft + Vector3.down * distance, Color.red);  // 左側面
        Debug.DrawLine(topRight, topRight + Vector3.down * distance, Color.red);  // 右側面
        Debug.DrawLine(bottomLeft, bottomLeft + Vector3.down * distance, Color.red);  // 下辺 (左)
        Debug.DrawLine(bottomRight, bottomRight + Vector3.down * distance, Color.red);  // 下辺 (右)

        return hit.collider != null;
    }
}
