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
    [SerializeField] float moveSpeed = 3.0f;                    //移動速度
    [SerializeField] float baseMoveSpeed = 3.0f;                //基礎移動速度
    [SerializeField] float jumpForce = 12.0f;                   //ジャンプ力
    [SerializeField] float dashForce = 10.0f;                   //ダッシュ力
    [SerializeField] float hookShotMoveSpeed = 0.5f;            //フックショットの移動速度

    [SerializeField] LayerMask groundLayer;                     //LayerのGround
    [SerializeField] GameObject hookShotObj;                    //フックショットのプレハブ
    [SerializeField] GameObject normalAttackRangeObj;           //通常攻撃範囲のオブジェクト
    [SerializeField] GameObject staminaNotationObj;             //スタミナゲージのオブジェクト
    [SerializeField] GameObject playerSummonObjects;            //プレイヤーが召喚できるオブジェクト
    [SerializeField] GameObject vacuumRangeObj;                 //吸い込み範囲のオブジェクト
    [SerializeField] GameObject debuffedAttributeResistanceObj; //デバフオブジェクト
    [SerializeField] GameObject playerArrowObj;                 //矢(など遠距離用)のオブジェクト

    [SerializeField] MainCameraController mainCameraControllerScript;   //メインカメラのスクリプト

    private bool isFollowUpAttack = false;                      //追撃トリガー(キャラ5用なので書き換えること)
    
    [SerializeField] List<GameObject> hookShotObjList = new List<GameObject>();     //フックショットのプレハブリスト
    [SerializeField] int hookShotObjCount = 0;                  //フックショットのカウント

    [SerializeField] GameObject afterEffectObj;                 //残像

    [SerializeField] int staminaRecoverySpeed = 60;             //スタミナ回復速度(1秒(60フレーム)で幾つ回復できるか)※1フレーム1回復が限度、数が大きいほど緩やかに回復

    [SerializeField] GameObject hpGaugeObj;                     //HPゲージのオブジェクト
    [SerializeField] GameObject mpGaugeObj;                     //MPゲージのオブジェクト

    [SerializeField] TeamCoutnroller teamCoutnrollerScript;     //TeamCoutnrollerのスクリプト

    [SerializeField] float invincibilityTimer = 0;              //無敵時間管理
    [SerializeField] float invincibilityCoolTime = 0.25f;       //無敵時間
    [SerializeField] float dashTimer = 0;                       //ダッシュ時間管理(3回以上連続でダッシュさせない)

    [SerializeField] float pairAnnihilationDamageCoolTime = 0.1f;   //対消滅ダメージのクールタイム
    [SerializeField] float pairAnnihilationDamageTimer = 0;     //対消滅ダメージのクールタイム

    [SerializeField] const float knockBackTime = 0.3f;          //ノックバックを受けた後の硬直時間
    private float currentKnockBackTime = 0f;                    //現在のノックバックの硬直時間
    private bool isStun = false;                                //硬直しているか

    [Header("キャラID")]
    [SerializeField] int charId = 1;                            //キャラクターのID
    [Header("属性")]
    [SerializeField] int attribute = 0;                         //属性(-1:回復、0:無属性(物理)、1:火、2:風、3:水、4:土、5:エーテル、6:虚空(ケノン))
    [Header("武器種")]
    [SerializeField] int weaponType = 0;                        //武器種
    [Header("レベル")]
    [SerializeField] byte charLevel = 1;                        //キャラレベル
    [Header("経験値量")]
    [SerializeField] int charExp = 0;                           //キャラ経験値
    [Header("最大スタミナ")]
    [SerializeField] int maxStamina = 100;                      //最大スタミナ
    [Header("現在スタミナ")]
    [SerializeField] int currentStamina;                        //現在スタミナ
    [Header("基礎HP")]
    [SerializeField] int baseHp = 100;                          //基礎HP
    [Header("最大HP")]
    [SerializeField] int maxHp = 100;                           //最大HP
    [Header("現在HP")]
    [SerializeField] int currentHp;                             //現在HP
    [Header("最大MP")]
    [SerializeField] int maxMp = 100;                           //最大MP
    [Header("現在MP")]
    [SerializeField] int currentMp;                             //現在MP
    [Header("基礎攻撃力")]
    [SerializeField] int baseAttack;                            //基礎攻撃力
    [Header("攻撃力(最終的)")]
    [SerializeField] int attack = 740;                          //攻撃力
    [Header("会心ダメージ")]
    [SerializeField] float attentionDamage = 200;               //会心ダメージ(攻撃+会心ダメージ(％))例)会心ダメージ150%は通常の2.5倍
    [Header("会心率")]
    [SerializeField] float attentionRate = 30;                  //会心率(上限は100%)
    [Header("必殺技のコスト")]
    [SerializeField] int maxSpecialMoveEnergy = 100;            //必殺技のコスト
    [Header("現在の必殺技のエネルギー量")]
    [SerializeField] int currentSpecialMoveEnergy = 100;        //現在の必殺技のコストがどのくらい溜まっているか
    [Header("必殺技のクールタイム")]
    [SerializeField] float maxSpecialMoveRecharge = 10.0f;      //必殺技のクールタイム設定(秒)
    [Header("必殺技のクールタイムの残り時間")]
    [SerializeField] float currentSpecialMoveRecharge = 0;      //現在の必殺技のクールタイム(次にスキルが使えるまでの時間)
    [Header("スキルのクールタイム")]
    [SerializeField] float maxSkillRecharge = 10.0f;            //スキルのクールタイム設定(秒)
    [Header("スキルのクールタイムの残り時間")]
    [SerializeField] float currentSkillRecharge = 0;            //現在のスキルのクールタイム(次にスキルが使えるまでの時間)

    //攻撃に関して、貫通するかと攻撃の寿命の項目を加えること(NormalAttackRangeでの振る舞いを変更する)
    //攻撃を関数にしておく、必要な引数を纏めること

    [Header("基礎攻撃バフ")]
    [SerializeField] int baseAttackBuff = 0;                    //基礎攻撃バフ(数値)
    [Header("攻撃バフ")]
    [SerializeField] float attackBuff = 1.0f;                   //攻撃バフ(％)
    [Header("HPバフ")]
    [SerializeField] float hpBuff = 1.0f;                       //HPバフ(％)
    [Header("ダメージバフ")]
    [SerializeField] float damageBuff = 1.0f;                   //ダメージバフ(％)

    [SerializeField] float damageReductionRate = 0f;            //ダメージ軽減率

    //Update関数内でバフの計算を毎フレーム行うこと
    //属性バフも加えること
    private bool isLookRight = true;                            //向き判定(右、前を向いているか)
    private bool isHookShotMoving = false;                      //フックショットで移動中か
    private bool isDash = false;                                //ダッシュ中か

    private int staminaRecoveryCounter = 0;                     //スタミナ回復処理のカウンター

    //ダメージ表記用
    [SerializeField] GameObject damageNotationObj;              //ダメージ表記オブジェクト
    [SerializeField] GameObject canvasObj;                      //Canvasを見つける(ダメージ表示用など)
    [SerializeField] Transform canvasTransform;                 //Canvasの座標等(ダメージ表示用など)

    //以降キャラ特有の変数

    //キャラID1の主人公用
    [SerializeField] int normalAttackAttribute = 0;             //通常攻撃の属性
    //キャラID2のシモラ用
    [SerializeField] bool isChar2Attention = false;             //確定会心

    //以上キャラ特有の変数でした

    //キャラクターデータベース
    public DB_CharData dB_charData;

    // Start is called before the first frame update
    void Start()
    {
        //FPS固定
        Application.targetFrameRate = 60;

        //ステータスをデータベースから参照
        CharDbReference();

        //ステータス代入
        currentHp = maxHp;
        currentMp = maxMp;
        currentStamina = maxStamina;

        //Canvasを見つける(ダメージ表示用など)
        canvasObj = GameObject.Find("DamageCanvas");
        canvasTransform = canvasObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //FPS計測
        //float fps = 1f / Time.deltaTime;
        //Debug.Log("FPS:" + fps);

        //攻撃力計算
        attack = Mathf.CeilToInt((float)((baseAttack + baseAttackBuff) * attackBuff) * damageBuff);
        //HP計算
        maxHp = Mathf.CeilToInt(baseHp * hpBuff);
        //基礎攻撃のリフレッシュ
        baseAttackBuff = 0;
        //攻撃力バフのリフレッシュ
        attackBuff = 1.0f;
        //HPバフのリフレッシュ
        hpBuff = 1.0f;
        //ダメージバフのリフレッシュ
        damageBuff = 1.0f;
        //ダメージ軽減率のリフレッシュ
        damageReductionRate = 0;

        //特殊移動(フックショット)中ではないとき
        if (isHookShotMoving == false)
        {
            //ダッシュ中か
            if (isDash == false) 
            {
                //移動
                MoveUpdate();
                //ダッシュ
                Dash();
            }

            //特殊移動(フックショット)...一部キャラの特殊移動？スキルにする予定
            HookShot();

            //通常攻撃
            NormalAttack(attack, normalAttackAttribute);           //攻撃力依存

            //Rayの判定(地上にいるとき)
            if (IsGrounding() == true)
            {
                //ジャンプ
                JumpUpdate();
            }
        }

        //特性
        Characteristic();

        //必殺技
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            SpecialMove();
        }
        //スキル
        if (Input.GetKeyDown(KeyCode.E))
        {
            Skill();
        }

        //死亡時
        if (CurrentHp == 0)
        {
            PlayerDeath();
        }

        //スタミナ回復(自然)
        Stamina();

        //無敵時間
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        //ダッシュ可能時間
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        //ダッシュ可能時間
        if (pairAnnihilationDamageTimer > 0)
        {
            pairAnnihilationDamageTimer -= Time.deltaTime;
        }

        //スキルクールタイム
        if (currentSkillRecharge > 0)
        {
            currentSkillRecharge -= Time.deltaTime;
        }
        //必殺技クールタイム
        if (currentSpecialMoveRecharge > 0)
        {
            currentSpecialMoveRecharge -= Time.deltaTime;
        }

        //値がマイナスになったとき0にする
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
        //値が最大値を超えたとき、規定値以下にする※なるべく呼び出されることがないように
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
            Debug.Log("HPオーバー");
        }
        if (currentMp > maxMp)
        {
            currentMp = maxMp;
            Debug.Log("MPオーバー");
        }
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
            Debug.Log("スタミナオーバー");
        }

        //奈落に落ちた場合の移動
        if (this.transform.position.y <= -100) 
        {
            rb2D.velocity = Vector3.zero;
            this.transform.position = new Vector2(0, 0);
        }

        //スタミナゲージの追従
        staminaNotationObj.transform.position = this.transform.position + new Vector3(0, -1.5f, 0);
    }

    //移動処理
    private void MoveUpdate()
    {
        //通常移動
        float moveX = Input.GetAxis("Horizontal"); // 水平方向の入力 (Aと←、Dと→)

        Vector2 moveDirection = new Vector2(moveX, 0);//二次元ベクトルを作成

        if (moveDirection.magnitude > 0.1f) // 入力が一定以上の場合のみ更新
        {
            moveDirection.Normalize(); // 斜め移動を一定速度にするために正規化
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90; // 回転角度を計算
            transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World); // 移動方向に沿って移動
        }
        //向きの変更
        if (moveX > 0)
        {
            Direction(true);
        }
        else if (moveX < 0)
        {
            Direction(false);
        }

        /*
        //無重力移動
        float moveX = Input.GetAxis("Horizontal"); // 水平方向の入力 (Aと←、Dと→)
        float moveY = Input.GetAxis("Vertical");   // 垂直方向の入力 (Wと↑、Sと↓)

        Vector2 moveDirection = new Vector2(moveX, moveY);//二次元ベクトルを作成

        if (moveDirection.magnitude > 0.1f) // 入力が一定以上の場合のみ更新
        {
            moveDirection.Normalize(); // 斜め移動を一定速度にするために正規化
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90; // 回転角度を計算
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World); // 移動方向に沿って移動
        }
        */
    }

    //向きの変更
    private void Direction(bool isLookRightInDirection)
    {
        isLookRight = isLookRightInDirection;
        Vector3 playerScale = this.transform.localScale;

        //スタミナゲージを反転させない
        //Transform staminaGaugeBaseTransform = this.transform.Find("StaminaGaugeBase");
        //Vector3 staminaGaugeBaseScale = staminaGaugeBaseTransform.localScale;

        //右向き
        if (isLookRight == true)
        {
            playerScale.x = 1 * Mathf.Abs(playerScale.x);
            //staminaGaugeBaseScale.x = 1 * Mathf.Abs(staminaGaugeBaseScale.x);
            //逆側に移動していた場合は止まる
            if (rb2D.velocity.x < 0)
            {
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }
        }
        //左向き
        else
        {
            playerScale.x = -1 * Mathf.Abs(playerScale.x);
            //staminaGaugeBaseScale.x = -1 * Mathf.Abs(staminaGaugeBaseScale.x);
            //逆側に移動していた場合は止まる
            if (rb2D.velocity.x > 0)
            {
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }
        }

        //staminaGaugeBaseTransform.localScale = staminaGaugeBaseScale;
        this.transform.localScale = playerScale;
    }

    //ジャンプ処理
    private void JumpUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //通常ジャンプ
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    //地面との接地判定
    private bool IsGrounding()
    {
        float rayLength = 0.6f;
        Vector2 direction = transform.right * 0.8f;
        Vector2 startPos = transform.position + new Vector3(-0.4f, -1.05f, 0);

        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, rayLength, groundLayer);
        Debug.DrawRay(startPos, direction, Color.red);

        return hit.collider != null;
    }

    //ダッシュ
    private void Dash()
    {
        if ((Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.LeftShift)) && currentStamina > 10 && dashTimer <= 0)  
        {
            //スタミナ消費
            ExhaustStamina(10);
            //無敵
            InvincibilityTimer(0.25f);
            //ダッシュ可能時間の更新
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
            //8方向対応
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
    //ダッシュ
    IEnumerator MoveDash(Vector2 moveDirection)
    {
        isDash = true;
        float thisGravity = rb2D.gravityScale;
        rb2D.gravityScale = 0;
        Vector2 thisVerocity = rb2D.velocity;
        rb2D.velocity = Vector2.zero;
        //透明化
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
                //残像の生成(GetComponentをなんとかする)
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

        //透明化解除
        spriteRenderer.color = thisColor;
        
        rb2D.gravityScale = thisGravity;
        //rb2D.velocity *= 0.1f;
        //rb2D.velocity += thisVerocity;
        rb2D.velocity = Vector2.zero;

        isDash = false;
        
        //空中にいる際は上向きに加速
        if (IsGrounding() == false) 
        {
            rb2D.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
        }

        yield break;
    }

    //特殊移動(フックショット)の処理
    private void HookShot()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.T) && currentStamina > 20)
        {
            //スタミナ消費
            ExhaustStamina(20);

            //移動先の目標オブジェクトの座標取得
            Vector2 mousePos = Input.mousePosition;
            //Debug.Log(mousePos);
            Camera camera = Camera.main;
            Vector2 touchScreenPos = camera.ScreenToWorldPoint(mousePos);
            //生成
            GameObject hookShotObjs = Instantiate(hookShotObj) as GameObject;
            hookShotObjs.transform.position = touchScreenPos;
            hookShotObjs.transform.rotation = Quaternion.identity;
            hookShotObjList.Add(hookShotObjs);
            hookShotObjCount++;

            //特殊移動(フックショット)の移動処理
            StartCoroutine(MoveHookShot(touchScreenPos));
        }
    }
    //特殊移動(フックショット)の移動処理
    IEnumerator MoveHookShot(Vector2 touchScreenPos)
    {
        //向きの計算
        Vector2 distance = (touchScreenPos - (Vector2)transform.position).normalized;

        //Debug.Log("distance" + distance + "touch" + touchScreenPos + "pos" + transform.position);

        //スタティックに変更(物理挙動を受け付けない)
        rb2D.bodyType = RigidbodyType2D.Static;
        //rb.bodyType = RigidbodyType2D.Kinematic;
        //特殊移動状態に変更
        isHookShotMoving = true;

        //向きの変更
        if (distance.x > 0)
        {
            Direction(true);
        }
        else
        {
            Direction(false);
        }

        //目標地点まで移動
        while (Vector2.Distance(touchScreenPos, (Vector2)transform.position) > 0.5)
        {
            transform.position += (Vector3)distance * hookShotMoveSpeed;
            yield return null;
            //向きの計算
            distance = (touchScreenPos - (Vector2)transform.position).normalized;
        }
        //スタティックから戻す(物理挙動を受け付けるように)
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        //速度を0に変更
        rb2D.velocity = Vector2.zero;
        //上向きに加速
        rb2D.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
        //特殊移動状態を解除
        isHookShotMoving = false;
        //移動目標先のオブジェクトの破壊
        if (hookShotObj == true)
        {
            Destroy(hookShotObjList[hookShotObjCount - 1]);
            hookShotObjList.RemoveAt(hookShotObjCount - 1);
            hookShotObjCount--;
            //Destroy(hookShotObj);
        }

        yield break;
    }

    //攻撃範囲の生成(通常やスキル、必殺技など含む全て)
    //キャラID、倍率計算後の攻撃力、属性、会心ダメージ、会心率、攻撃発生場所、攻撃範囲の大きさ(x,y)、ノックバック量(charIDが抜けているので加えること)、追撃か(追撃ならtrue、それ以外はfalse)
    public void AttackMaker(int multipliedAttack, int finalAttributeNormalAttack, float multipliedAttentionDamage, float multipliedAttentionRate, Vector3 attackPos, Vector2 attackSize, float knockBackValue, bool isFollowUpAttack)
    {
        //char2が確定会心のとき(移動させること)
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

    //追撃(PlayerArrowObjectsにも単体攻撃があるのでPlayer側の関数で纏めること)
    public void FollowUpAttack(EnemyHP enemyHpScript) 
    {
        //OnFollowUpAttack?.Invoke(onFollowUpAttackMultipliedAttack, onFollowUpAttackFinalAttributeNormalAttack, onFollowUpAttackMultipliedAttentionDamage, onFollowUpAttackMultipliedAttentionRate, onFollowUpAttackAttackPos, onFollowUpAttackAttackSize, onFollowUpAttackKnockBackValue, onFollowUpAttackIsFollowUpAttack);
        if (isFollowUpAttack == true) //攻撃力を固定にすること
        {
            //汎用
            //SingleAttack(enemyHpScript, multipliedAttack, finalAttribute, multipliedAttentionDamage, multipliedAttentionRate, knockBackValue, isFollowUpAttack);
            SingleAttack(enemyHpScript, (int)(attack * 0.3f), 6, attentionDamage, attentionRate, 0, true);
        }
    }

    //単体攻撃
    public void SingleAttack(EnemyHP enemyHpScript, int multipliedAttack, int finalAttribute, float multipliedAttentionDamage, float multipliedAttentionRate, float knockBackValue, bool isFollowUpAttack) 
    {
        bool isAttentionDamage = false;         //会心ダメージか
        //会心率の抽選
        float randomPoint = UnityEngine.Random.value * 100;
        if (randomPoint <= multipliedAttentionRate)
        {
            multipliedAttack = (int)((float)(multipliedAttack) * ((100 + multipliedAttentionDamage) / 100));
            isAttentionDamage = true;
        }
        //攻撃したキャラのID(厳密にするならcharIdを追跡して受け取っても良い)、ダメージ判定のx座標、攻撃力、属性、会心かどうか、追撃かどうか(追撃がtrue)
        enemyHpScript.EnemyDamage(charId, this.transform.position.x, multipliedAttack, finalAttribute, isAttentionDamage, knockBackValue, isFollowUpAttack);
    }

    //回復
    public void Heal(bool isTeamHeal, float healValue) 
    {
        //全体回復
        if (isTeamHeal == true)
        {
            for (int i = 0; i < 3; i++)
            {
                if (teamCoutnrollerScript.TeamCurrentHp[i] != 0)
                {
                    //自分の回復
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
                        //ダメージ表記呼び出し
                        var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
                        DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
                        //引数は攻撃力、属性、会心判定(無)、表記の発生場所(味方の被ダメージに会心ダメージは発生しない)
                        damageNotationObjsScript.DamageNotion(Mathf.CeilToInt(healValue), -1, false, (Vector2)transform.position + new Vector2(0, 0.0f));
                    }
                    //自分以外の回復
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
                        //ダメージ表記呼び出し
                        var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
                        DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
                        //引数は攻撃力、属性、会心判定(無)、表記の発生場所(味方の被ダメージに会心ダメージは発生しない)
                        damageNotationObjsScript.DamageNotion(Mathf.CeilToInt(healValue), -1, false, (Vector2)transform.position + new Vector2(0, 0.0f));
                    }
                }
            }
        }
        //単体回復
        else
        {
            //自分の回復
            if (currentHp + Mathf.CeilToInt(healValue) >= maxHp)
            {
                currentHp = maxHp;
            }
            else
            {
                currentHp += Mathf.CeilToInt(healValue);
            }
            //ダメージ表記呼び出し
            var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
            DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
            //引数は攻撃力、属性、会心判定(無)、表記の発生場所(味方の被ダメージに会心ダメージは発生しない)
            damageNotationObjsScript.DamageNotion(Mathf.CeilToInt(healValue), -1, false, (Vector2)transform.position + new Vector2(0, 0.0f));
        }
    }

    //集敵(固有スキルなどなのでリファクタリングでは別スクリプトにしておくこと)
    //集敵発生場所、集敵範囲、集敵持続時間(秒)
    public void Vacuum(Vector2 vacuumPos, Vector2 vacuumSize, float vacuumDuration, float VacuumPower) 
    {
        //吸い込み
        var vacuumRangeObjs = Instantiate(vacuumRangeObj, vacuumPos, this.transform.rotation);
        vacuumRangeObjs.transform.localScale = new Vector3(vacuumSize.x, vacuumSize.y, 1.0f);
        VacuumRange vacuumRangeObjsScript = vacuumRangeObjs.GetComponent<VacuumRange>();
        vacuumRangeObjsScript.Vacuum(vacuumDuration, VacuumPower);
    }

    //属性耐性ダウン
    //耐性ダウン値(%)、生成位置、デバフ範囲、持続時間がデバフ(時間依存を一瞬で掛けるtrue)かデバフ範囲か(出たら消えるflase)、持続時間、攻撃ID
    public void DebuffedAttributeResistance(float debuffedAttributeResistance, Vector2 pos, Vector2 size, bool isTypeMoment, float duration, int debuffedId) 
    {
        //％を割合変換
        debuffedAttributeResistance /= 100;
        //耐性ダウン
        var debuffedAttributeResistanceObjs = Instantiate(debuffedAttributeResistanceObj, pos, this.transform.rotation);
        debuffedAttributeResistanceObjs.transform.localScale = new Vector3(size.x, size.y, 1.0f);
        DebuffedAttributeResistance debuffedAttributeResistanceObjsScript = debuffedAttributeResistanceObjs.GetComponent<DebuffedAttributeResistance>();
        debuffedAttributeResistanceObjsScript.Debuffed(debuffedAttributeResistance, isTypeMoment, duration, charId, debuffedId);
    }

    //プレイヤーの召喚物(設置スキル)
    //継続時間(summonDuration)、ノックバック量、生成位置、サイズ
    public void PlayerSummonObjects(float summonDuration, float summonKnockBackValue, Vector2 playerSummonObjsPos, Vector2 playerSummonObjsSize)
    {
        //設置
        var playerSummonObjs = Instantiate(playerSummonObjects, playerSummonObjsPos, this.transform.rotation);
        playerSummonObjs.transform.localScale = new Vector3(playerSummonObjsSize.x, playerSummonObjsSize.y, 1.0f);
        PlayerSummonObjects playerSummonObjectsScript = playerSummonObjs.GetComponent<PlayerSummonObjects>();
        playerSummonObjectsScript.Summon(charId, summonDuration, attack, attribute, maxHp, attentionDamage, attentionRate, summonKnockBackValue);
    }

    //キャラID、倍率計算後の攻撃力、属性、会心ダメージ、会心率、攻撃発生場所、攻撃範囲の大きさ(x,y)、ノックバック量(charIDが抜けているので加えること)
    //public void AttackMaker(int multipliedAttack, int finalAttributeNormalAttack, float multipliedAttentionDamage, float multipliedAttentionRate, Vector3 attackPos, Vector2 attackSize, float knockBackValue)
    //プレイヤーの矢(遠距離攻撃)
    //攻撃タイプ(通常0、ID的なもの)、攻撃力、属性、会心ダメージ、会心率、矢の発生位置、矢の大きさ、攻撃範囲(0の場合は単体攻撃)、ノックバック量、発射角度
    public void Arrow(int attackType, int multipliedAttack, int arrowAttribute, float arrowAttentionDamage, float arrowAttentionRate, Vector2 playerArrowObjsPos, Vector2 playerArrowObjsSize, Vector2 arrowAttackRangeSize, float arrowKnockBackValue, float arrowLaunchAngle)
    {
        //設置
        var playerArrowObjs = Instantiate(playerArrowObj, playerArrowObjsPos, this.transform.rotation);
        playerArrowObjs.transform.localScale = new Vector3(playerArrowObjsSize.x, playerArrowObjsSize.y, 1.0f);
        PlayerArrowObject playerArrowObjectScript = playerArrowObjs.GetComponent<PlayerArrowObject>();
        playerArrowObjectScript.Arrow(GetComponent<Player>(), charId, attackType, multipliedAttack, arrowAttribute, arrowAttentionDamage, arrowAttentionRate, arrowAttackRangeSize, arrowKnockBackValue, arrowLaunchAngle);
    }

    //通常攻撃(攻撃力,属性)
    private void NormalAttack(int attack, int attributeNormalAttack)
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.E) == false)
        {
            //右向きと左向きで発生位置を変更
            float normalAttackDirection = 0.75f;
            normalAttackDirection *= GetFacingDirection(isLookRight);
            Vector3 normalAttackPos = this.transform.position;
            normalAttackPos.x += normalAttackDirection;

            AttackMaker(attack, attributeNormalAttack, attentionDamage, attentionRate, normalAttackPos, new Vector2(2.0f, 2.0f), 100, false);
        }
    }

    //必殺技
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

    //キャラIDが1のキャラの必殺技
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

    //キャラIDが2のキャラの必殺技
    async public void Char2SpecialMove()
    {
        rb2D.AddForce(new Vector2(0,7.5f), ForceMode2D.Impulse);
        
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        
        rb2D.bodyType = RigidbodyType2D.Static;
        //〇秒掛けて1回転のように記述
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

    //キャラIDが3のキャラの必殺技
    public void Char3SpecialMove()
    {
        PlayerSummonObjects(10, 0, this.transform.position, new Vector2(0.5f, 0.5f));
    }

    //キャラIDが4のキャラの必殺技
    public void Char4SpecialMove()
    {
        PlayerSummonObjects(10, 0, this.transform.position, new Vector2(15.0f, 15.0f));
    }

    //キャラIDが5のキャラの必殺技
    async public void Char5SpecialMove()
    {
        AttackMaker((int)(attack * 3.6f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(7.5f, 7.5f), 300, false);

        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

        isFollowUpAttack = true;
        
        await UniTask.Delay(TimeSpan.FromSeconds(15.0f));

        isFollowUpAttack = false;
    }

    //キャラIDが6のキャラの必殺技
    async public void Char6SpecialMove()
    {
        DebuffedAttributeResistance(20, transform.position, new Vector2(12.0f, 12.0f), false, 12.5f, 1);
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        AttackMaker((int)(attack * 14.5f), 3, attentionDamage, attentionRate, this.transform.position, new Vector2(5.5f, 5.5f), 800, false);
        mainCameraControllerScript.ShakeCamera(0.25f, 0.5f, 90, 15, false, true);
    }

    //キャラIDが7のキャラの必殺技
    public void Char7SpecialMove()
    {
        
    }

    //キャラIDが8のキャラの必殺技
    public void Char8SpecialMove()
    {
        
    }

    //スキル
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

    //キャラIDが1のキャラのスキル
    async public void Char1Skill()
    {
        //5.0秒間通常攻撃をエーテル属性に変化(バグ対応をすること(控えに戻ったらキャンセル))
        normalAttackAttribute = 5;

        await UniTask.Delay(TimeSpan.FromSeconds(7.0f));

        //通常攻撃を物理属性に戻す
        normalAttackAttribute = 0;
    }

    //キャラIDが2のキャラのスキル
    public void Char2Skill()
    {
        //HP半減(切り上げ)
        currentHp = (int)Mathf.Ceil((float)(currentHp) / 2);
        //次の攻撃が確定会心
        isChar2Attention = true;
    }

    //キャラIDが3のキャラのスキル
    public void Char3Skill()
    {
        
        //集敵効果
        Vacuum(this.transform.position, new Vector2(12.5f, 12.5f), 0.5f, 0.1f);
        //攻撃
        AttackMaker((int)(attack * 4.5f), 3, attentionDamage, attentionRate, this.transform.position, new Vector2(10.0f, 10.0f), 0, false);
    }

    //キャラIDが4のキャラのスキル
    public void Char4Skill()
    {
        for(int i = 0; i < 9; i++) 
        {
            Arrow(i + 1, (int)(attack * 0.7f), attribute, attentionDamage, attentionRate, this.transform.position, new Vector2(0.2f, 0.2f), new Vector2(1.0f, 1.0f), 10, 0);
        }
    }

    //キャラIDが5のキャラのスキル
    public void Char5Skill()
    {
        Vector2 vacuumPos = new Vector2(transform.position.x + GetFacingDirection(isLookRight) * 3.0f, transform.position.y);
        //集敵効果
        Vacuum(vacuumPos, new Vector2(7.5f, 7.5f), 5.0f, 0.1f);
    }

    //キャラIDが6のキャラのスキル
    async public void Char6Skill()
    {
        float healValue = maxHp * 0.05f;
        for(int i = 0; i < 6; i++) 
        {
            Heal(true, healValue);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        }
    }

    //キャラIDが7のキャラのスキル
    public void Char7Skill()
    {
        
    }

    //キャラIDが8のキャラのスキル
    async public void Char8Skill()
    {
        for (int i = 0; i < 10; i++) 
        {
            int randomAttribute = UnityEngine.Random.Range(0, 7);
            Arrow(1, (int)(attack * 1.2f), randomAttribute, attentionDamage, attentionRate, this.transform.position, new Vector2(0.2f, 0.2f), new Vector2(0, 0), 10, 0);
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
        }
    }

    //特性
    private void Characteristic()
    {
        //チームキャラの把握
        int[] teamId = { teamCoutnrollerScript.TeamIdData[0], teamCoutnrollerScript.TeamIdData[1], teamCoutnrollerScript.TeamIdData[2] };

        if (charId == 1)
        {
            //Char1Characteristic();
        }
        if (charId == 2)
        {
            Char2Characteristic();
        }
        if (teamId.Contains(3)) //控えから発動可能 
        {
            Char3Characteristic();
        }
        if (teamId.Contains(4)) //控えから発動可能
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

    //キャラIDが1のキャラの特性
    public void Char1Characteristic()
    {
        //敵を倒したときHP回復
        Heal(false, maxHp * 0.15f);
    }

    //キャラIDが2のキャラの特性
    public void Char2Characteristic()
    {
        //HPが半分以下のとき攻撃力2倍
        if (currentHp <= Mathf.Ceil((float)maxHp / 2)) 
        {
            //ダメージ2倍
            damageBuff *= 2.0f;
        }
    }

    //キャラIDが3のキャラの特性
    public void Char3Characteristic()
    {
        //ダメージ軽減
        DamageReduction(25.0f);
    }

    //キャラIDが4のキャラの特性
    public void Char4Characteristic()
    {
        //控えからでも継続させること
        int tA1 = dB_charData.charData[teamCoutnrollerScript.TeamIdData[0]].attribute;
        int tA2 = dB_charData.charData[teamCoutnrollerScript.TeamIdData[1]].attribute;
        int tA3 = dB_charData.charData[teamCoutnrollerScript.TeamIdData[2]].attribute;
        //3属性異なる
        if (tA1 != tA2 && tA2 != tA3 && tA3 != tA1)
        {
            damageBuff *= 1.4f;
        }
        //2属性異なる
        else if ((tA1 == tA2 && tA2 == tA3 && tA3 == tA1) == false) 
        {
            damageBuff *= 1.2f;
        }
    }

    //キャラIDが5のキャラの特性
    public void Char5Characteristic()
    {
        //
    }

    //キャラIDが6のキャラの特性
    public void Char6Characteristic()
    {
        baseAttackBuff += Mathf.CeilToInt(currentHp * 0.025f);
    }

    //キャラIDが7のキャラの特性
    public void Char7Characteristic()
    {
        
    }

    //キャラIDが8のキャラの特性
    public void Char8Characteristic()
    {
        
    }

    //被ダメージ
    public void Damage(EnemyAction enemyAction, int damage, int enemyAttribute)
    {
        //無敵ではないとき
        if (invincibilityTimer <= 0)
        {
            damage = GameSystemUtility.CalcDamage(damage, enemyAttribute, attribute, () => StartCoroutine(PairAnnihilationDamage(enemyAction, damage)));

            //ダメージ軽減
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

            //ダメージ表記呼び出し
            var damageNotationObjs = Instantiate<GameObject>(damageNotationObj, transform.position, Quaternion.identity, canvasTransform);
            DamageNotation damageNotationObjsScript = damageNotationObjs.GetComponent<DamageNotation>();
            //引数は攻撃力、属性、会心判定(無)、表記の発生場所(味方の被ダメージに会心ダメージは発生しない)
            damageNotationObjsScript.DamageNotion(damage, enemyAttribute, false, (Vector2)transform.position + new Vector2(0, 0.0f));

            //DamageNotationCountroller damageNotationObjScript = damageNotationObj.GetComponent<DamageNotationCountroller>();
            //damageNotationObjScript.DamageNotation(damage, (Vector2)transform.position + new Vector2(0, 1.0f));
            //Debug.Log("Damaged" + damage);
            //Debug.Log("CurrentHP" + currentHp);

            //無敵時間の更新
            InvincibilityTimer(invincibilityCoolTime);
        }
    }

    //ダメージ軽減計算
    public void DamageReduction(float damageReductionRateValue) 
    {
        damageReductionRate += damageReductionRateValue;
    }

    //硬直
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

    //対消滅ダメージ
    IEnumerator PairAnnihilationDamage(EnemyAction enemyAction, int damage)
    {
        if (pairAnnihilationDamageTimer <= 0) 
        {
            Debug.Log("対消滅ダメージ");
            if (enemyAction == true)
            {
                enemyAction.EnemyAttack();
            }
            pairAnnihilationDamageTimer = pairAnnihilationDamageCoolTime;
        }
        yield break;
    }

    //無敵時間
    public void InvincibilityTimer(float timer) 
    {
        if (invincibilityTimer <= timer) 
        {
            invincibilityTimer = timer;
        }
    }

    //スタミナ回復(自然)
    private void Stamina()
    {
        staminaRecoveryCounter++;

        if (currentStamina < maxStamina && staminaRecoveryCounter >= 60 / staminaRecoverySpeed)
        {
            currentStamina++;
            staminaRecoveryCounter = 0;
        }
    }

    //スタミナ消費
    private void ExhaustStamina(int exhaustStamina)
    {
        currentStamina -= exhaustStamina;
    }

    //キャラクターチェンジ
    public void CharChange(int changeCharId) //次の表に出るキャラのID
    {
        charId = changeCharId;

        //ここにステータス等の読み込みを記述
        CharDbReference();
    }

    //キャラ情報をデータベースから参照
    public void CharDbReference() 
    {
        attribute = dB_charData.charData[charId].attribute;                 //属性
        baseHp = dB_charData.charData[charId].baseHp;                       //基礎HP
        baseAttack = dB_charData.charData[charId].baseAttack;               //基礎攻撃力
        maxSkillRecharge = dB_charData.charData[charId].maxSkillRecharge;   //スキルクールタイム
        maxSpecialMoveRecharge = dB_charData.charData[charId].maxSpecialMoveRecharge;   //必殺技クールタイム
    }

    //プレイヤー死亡
    private void PlayerDeath()
    {
        Debug.Log("PlayerDeath");
    }

    //敵を倒した
    public void EnemyKill() 
    {
        if (charId == 1)
        {
            Char1Characteristic();
        }
        Debug.Log("EnemyKill");
    }

    /*
    //条件付き
    public int CurrentHP
    {
        get { return currentHp; } //getterの部分
        set
        {
            if (value >= 0)  //setterの部分、100を超えたときのみ代入する
                currentHp = value;
        }
    }
    */

    //maxHp参照用(getset)
    public int MaxHp // プロパティ
    {
        get { return maxHp; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { maxHp = value; } // 通称セッター。value はセットする側の数字などを反映する
    }
    //public int MaxHp => maxHp;

    //currentHp参照用(getset)
    public int CurrentHp // プロパティ
    {
        get { return currentHp; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { currentHp = value; } // 通称セッター。value はセットする側の数字などを反映する
    }

    //maxMp参照用(getset)
    public int MaxMp // プロパティ
    {
        get { return maxMp; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { maxMp = value; } // 通称セッター。value はセットする側の数字などを反映する
    }

    //currentMp参照用(getset)
    public int CurrentMp // プロパティ
    {
        get { return currentMp; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { currentMp = value; } // 通称セッター。value はセットする側の数字などを反映する
    }

    //maxStamina参照用(getset)
    public int MaxStamina // プロパティ
    {
        get { return maxStamina; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { maxStamina = value; } // 通称セッター。value はセットする側の数字などを反映する
    }

    //currentStamina参照用(getset)
    public int CurrentStamina // プロパティ
    {
        get { return currentStamina; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { currentStamina = value; } // 通称セッター。value はセットする側の数字などを反映する
    }

    //currentSkillRecharge参照用(getset)
    public float CurrentSkillRecharge // プロパティ
    {
        get { return currentSkillRecharge; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { currentSkillRecharge = value; } // 通称セッター。value はセットする側の数字などを反映する
    }
    
    //currentSpecialMoveRecharge参照用(getset)
    public float CurrentSpecialMoveRecharge // プロパティ
    {
        get { return currentSpecialMoveRecharge; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { currentSpecialMoveRecharge = value; } // 通称セッター。value はセットする側の数字などを反映する
    }

    /*
    //スクリプトA(参照先)
    public int Life // プロパティ
    {
        get { return life; }  // 通称ゲッター。呼び出した側がscoreを参照できる
        set { life = value; } // 通称セッター。value はセットする側の数字などを反映する
    }
    //スクリプトB(参照する方)
    int life = ball.Life; // ゲッター。ScriptAの変数を取得する
    */

    //キャラID受け渡し
    public int CharId => charId;
    //攻撃力受け渡し
    public int Attack => attack;
    //会心率受け渡し
    public float AttentionDamage => attentionDamage;
    //会心率受け渡し
    public float AttentionRate => attentionRate;
    //右を向いているかの判定の受け渡し
    public bool IsLookRight => isLookRight;
}
