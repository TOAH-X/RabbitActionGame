using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CharData
{
    [Header("キャラID")]
    public int charId;                                  //キャラクターのID
    [Header("キャラ名")]
    public string charName;                             //キャラクター名
    [Header("レア度")]
    public string rarity;                               //レア度
    [Header("属性")]
    public int attribute;                               //属性(0:無属性(物理)、1:火、2:風、3:水、4:土、5:エーテル、6:虚空(ケノン))
    [Header("武器種")]
    public int weaponType = 0;                          //武器種
    //public byte charLevel;                            //キャラレベル
    //public int charExp;                               //キャラ経験値
    //public int maxStamina;                            //最大スタミナ
    [Header("基礎HP")]
    public int baseHp;                                  //基礎HP
    //public int maxHp;                                 //最大HP
    [Header("HPバフ")]
    public float buffHp;                                //HPバフ
    //public int currentHp;                             //現在HP
    //public int maxMp;                                 //最大MP
    //public int currentMp;                             //現在MP
    [Header("基礎攻撃力")]
    public int baseAttack;                              //基礎攻撃力
    //public int attack;                                //攻撃力
    //public float buffAttack;                          //攻撃力バフ
    [Header("基礎会心ダメージ")]
    public float baseAttentionDamage = 150;             //基礎会心ダメージ(攻撃+会心ダメージ(％))例)会心ダメージ150%は通常の2.5倍
    [Header("基礎会心率")]
    public float baseAttentionRate = 30;                //基礎会心率(上限は100%)

    [Header("特性名")]
    public string characteristicName;                   //特性名
    [Header("特性説明"), TextArea]
    public string characteristicExplanation;            //特性説明
    [Header("スキル名")]
    public string skillName;                            //スキル名
    [Header("スキル説明"), TextArea]
    public string skillExplanation;                     //スキル説明
    [Header("必殺技名")]
    public string specialMoveName;                      //必殺技名
    [Header("必殺技説明"), TextArea]
    public string specialMoveExplanation;               //必殺技説明

    [Header("スキルクールタイム")]
    public float maxSkillRecharge;                      //スキルのクールタイム(秒)
    [Header("必殺技コスト")]
    public int maxSpecialMoveEnergy;                    //必殺技のコスト
    [Header("必殺技クールタイム")]
    public int maxSpecialMoveRecharge;                  //必殺技のクールタイム(秒)

    //public int weaponId = 0;                          //装備している武器のID

    //public int sousinguId = 0;                        //装備している装身具(聖遺物)のID

    [Header("基礎移動速度")]
    public float baseMoveSpeed = 3.0f;                  //基礎移動速度
    //public float buffMoveSpeed = 3.0f;                //移動速度バフ
    //public float jumpForce = 12.0f;                   //ジャンプ力
    //public float dashForce = 5.0f;                    //ダッシュ力

    [Header("アイコン画像")]
    public Sprite charIcon;                              //アイコン画像
    [Header("立ち絵")]
    public Sprite charFullBodyImage;                     //立ち絵
    //[Header("ゲーム内キャラ")]
    //public Sprite charAnimation;                              //ゲーム内キャラ
}
