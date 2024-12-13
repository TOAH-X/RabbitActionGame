using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int enemyId = 1;                           //“G‚ÌID
    [SerializeField] int enemyAttribute = 0;                    //“G‚Ì‘®«ADBŽQÆ
    [SerializeField] float enemyMaxHpBaseRate;                  //“G‚ÌŠî‘bHP”{—¦
    [SerializeField] float enemyAttackBaseRate;                 //“G‚ÌŠî‘bUŒ‚—Í”{—¦

    [SerializeField] int enemyLevel = 99;                       //“G‚ÌƒŒƒxƒ‹

    [SerializeField] int enemyMaxHp;                            //“G‚ÌHP
    [SerializeField] int enemyAttack;                           //“G‚ÌUŒ‚—Í

    [SerializeField] Sprite enemyPicture;                       //“G‚Ì‰æ‘œ

    //ƒLƒƒƒ‰ƒNƒ^[ƒf[ƒ^ƒx[ƒX
    public DB_EnemyData dB_enemyData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ƒXƒ|ƒi[‚©‚ç“G‚Ìƒf[ƒ^‚ðŽó‚¯Žæ‚é
    public void EnemyData(int iD, int level)
    {
        enemyId = iD;
        enemyLevel = level;

        //“G‚ÌDB‚ÌŽQÆ
        EnemyDbReference();

        //Å‘åHP‚ÌŒvŽZ
        enemyMaxHp = Mathf.RoundToInt(enemyMaxHpBaseRate * (10.0f * Mathf.Pow(enemyLevel, 2) + 100.0f * enemyLevel + 100));
        //UŒ‚—Í‚ÌŒvŽZ
        enemyAttack = Mathf.RoundToInt(enemyMaxHpBaseRate * (0.01f * Mathf.Pow(enemyLevel, 2) + 10.0f * enemyLevel + 10));
    }

    //“Gî•ñ‚ðƒf[ƒ^ƒx[ƒX‚©‚çŽQÆ
    public void EnemyDbReference()
    {
        enemyMaxHpBaseRate = dB_enemyData.enemyData[enemyId].enemyMaxHpBaseRate;    //“G‚ÌŠî‘bHP”{—¦
        enemyAttackBaseRate = dB_enemyData.enemyData[enemyId].enemyAttackBaseRate;  //“G‚ÌŠî‘bUŒ‚—Í”{—¦
        enemyAttribute = dB_enemyData.enemyData[enemyId].enemyAttribute;            //“G‚Ì‘®«
        enemyPicture = dB_enemyData.enemyData[enemyId].enemyPicture;                //“G‚Ì‰æ‘œ
    }

    //“GIDŽó‚¯“n‚µ
    public int EnemyId => enemyId;
    //“G‘®«Žó‚¯“n‚µ
    public int EnemyAttribute => enemyAttribute;
    //“G‚ÌÅ‘åHPŽó‚¯“n‚µ
    public int EnemyMaxHp => enemyMaxHp;
    //“G‚ÌUŒ‚—ÍŽó‚¯“n‚µ
    public int EnemyAttack => enemyAttack;

    //“G‚ÌUŒ‚—ÍŽó‚¯“n‚µ
    public Sprite EnemyPicture => enemyPicture;


    //“GƒŒƒxƒ‹Žó‚¯“n‚µ
    public int EnemyLevel => enemyLevel;

}
