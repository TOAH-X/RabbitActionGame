using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
    [Header("敵キャラID")]
    public int enemyId = 1;                             //敵のID(0はデモ用なので1から始める)
    [Header("敵キャラ名")]
    public String enemyName;                            //敵の名前
    [Header("種族名")]
    public String enemyRace;                            //敵の種族
    [Header("敵の属性")]
    public int enemyAttribute = 1;                      //敵の属性
    [Header("敵の攻撃力")]
    public int enemyAttack = 100;                       //敵の攻撃力(厳密には基礎攻撃力)
    [Header("敵の攻撃力の基礎倍率")]
    public float enemyAttackBaseRate = 1;               //敵の攻撃力の基礎倍率
    [Header("敵の最大HP")]
    public int enemyMaxHp = 10000;                      //敵の最大HP
    [Header("敵の最大HPの基礎倍率")]
    public float enemyMaxHpBaseRate = 1;                //敵の最大HPの基礎倍率
    [Header("敵イラスト")]
    public Sprite enemyPicture;                         //敵のイラスト
}
