using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int enemyId = 1;                           //GÌID
    [SerializeField] int enemyAttribute = 0;                    //GÌ®«ADBQÆ
    [SerializeField] float enemyMaxHpBaseRate;                  //GÌîbHP{¦
    [SerializeField] float enemyAttackBaseRate;                 //GÌîbUÍ{¦

    [SerializeField] int enemyLevel = 99;                       //GÌx

    [SerializeField] int enemyMaxHp;                            //GÌHP
    [SerializeField] int enemyAttack;                           //GÌUÍ

    [SerializeField] Sprite enemyPicture;                       //GÌæ

    [SerializeField] SpriteRenderer spriteRenderer;             //GÌXvCg_[

    //LN^[f[^x[X
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

    //X|i[©çGÌf[^ðó¯æé
    public void EnemyData(int iD, int level)
    {
        enemyId = iD;
        enemyLevel = level;
        //spriteRenderer = GetComponent<SpriteRenderer>();
        

        //GÌDBÌQÆ
        EnemyDbReference();

        //ÅåHPÌvZ
        enemyMaxHp = Mathf.RoundToInt(enemyMaxHpBaseRate * (10.0f * Mathf.Pow(enemyLevel, 2) + 100.0f * enemyLevel + 100));
        //UÍÌvZ
        enemyAttack = Mathf.RoundToInt(enemyAttackBaseRate * (0.01f * Mathf.Pow(enemyLevel, 2) + 10.0f * enemyLevel + 10));
    }

    //Gîñðf[^x[X©çQÆ
    public void EnemyDbReference()
    {
        enemyMaxHpBaseRate = dB_enemyData.enemyData[enemyId].enemyMaxHpBaseRate;    //GÌîbHP{¦
        enemyAttackBaseRate = dB_enemyData.enemyData[enemyId].enemyAttackBaseRate;  //GÌîbUÍ{¦
        enemyAttribute = dB_enemyData.enemyData[enemyId].enemyAttribute;            //GÌ®«
        enemyPicture = dB_enemyData.enemyData[enemyId].enemyPicture;                //GÌæ
    }

    //GIDó¯nµ
    public int EnemyId => enemyId;
    //G®«ó¯nµ
    public int EnemyAttribute => enemyAttribute;
    //GÌÅåHPó¯nµ
    public int EnemyMaxHp => enemyMaxHp;
    //GÌUÍó¯nµ
    public int EnemyAttack => enemyAttack;

    //GÌUÍó¯nµ
    public Sprite EnemyPicture => enemyPicture;


    //Gxó¯nµ
    public int EnemyLevel => enemyLevel;

}
