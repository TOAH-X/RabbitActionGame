using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int enemyId = 1;                           //“G‚ÌID
    [SerializeField] int enemyAttribute = 0;                    //“G‚Ì‘®«ADBQÆ
    [SerializeField] float enemyMaxHpBaseRate;                  //“G‚ÌŠî‘bHP”{—¦
    [SerializeField] float enemyAttackBaseRate;                 //“G‚ÌŠî‘bUŒ‚—Í”{—¦

    [SerializeField] int enemyLevel = 99;                       //“G‚ÌƒŒƒxƒ‹

    [SerializeField] int enemyMaxHp;                            //“G‚ÌHP
    [SerializeField] int enemyAttack;                           //“G‚ÌUŒ‚—Í

    [SerializeField] Sprite enemyPicture;                       //“G‚Ì‰æ‘œ

    [SerializeField] SpriteRenderer spriteRenderer;             //“G‚ÌƒXƒvƒ‰ƒCƒgƒŒƒ“ƒ_ƒ‰[

    //ƒLƒƒƒ‰ƒNƒ^[ƒf[ƒ^ƒx[ƒX
    public DB_EnemyData dB_enemyData;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = enemyPicture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ƒXƒ|ƒi[‚©‚ç“G‚Ìƒf[ƒ^‚ğó‚¯æ‚é
    public void EnemyData(int iD, int level)
    {
        enemyId = iD;
        enemyLevel = level;
        //spriteRenderer = GetComponent<SpriteRenderer>();
        

        //“G‚ÌDB‚ÌQÆ
        EnemyDbReference();

        //Å‘åHP‚ÌŒvZ
        enemyMaxHp = Mathf.RoundToInt(enemyMaxHpBaseRate * (10.0f * Mathf.Pow(enemyLevel, 2) + 100.0f * enemyLevel + 100));
        //UŒ‚—Í‚ÌŒvZ
        enemyAttack = Mathf.RoundToInt(enemyMaxHpBaseRate * (0.01f * Mathf.Pow(enemyLevel, 2) + 10.0f * enemyLevel + 10));
    }

    //“Gî•ñ‚ğƒf[ƒ^ƒx[ƒX‚©‚çQÆ
    public void EnemyDbReference()
    {
        enemyMaxHpBaseRate = dB_enemyData.enemyData[enemyId].enemyMaxHpBaseRate;    //“G‚ÌŠî‘bHP”{—¦
        enemyAttackBaseRate = dB_enemyData.enemyData[enemyId].enemyAttackBaseRate;  //“G‚ÌŠî‘bUŒ‚—Í”{—¦
        enemyAttribute = dB_enemyData.enemyData[enemyId].enemyAttribute;            //“G‚Ì‘®«
        enemyPicture = dB_enemyData.enemyData[enemyId].enemyPicture;                //“G‚Ì‰æ‘œ
    }

    //“GIDó‚¯“n‚µ
    public int EnemyId => enemyId;
    //“G‘®«ó‚¯“n‚µ
    public int EnemyAttribute => enemyAttribute;
    //“G‚ÌÅ‘åHPó‚¯“n‚µ
    public int EnemyMaxHp => enemyMaxHp;
    //“G‚ÌUŒ‚—Íó‚¯“n‚µ
    public int EnemyAttack => enemyAttack;

    //“G‚ÌUŒ‚—Íó‚¯“n‚µ
    public Sprite EnemyPicture => enemyPicture;


    //“GƒŒƒxƒ‹ó‚¯“n‚µ
    public int EnemyLevel => enemyLevel;

}
