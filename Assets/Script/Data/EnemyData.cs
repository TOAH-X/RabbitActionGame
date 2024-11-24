using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
    [Header("�G�L����ID")]
    public int enemyId = 1;                             //�G��ID(0�̓f���p�Ȃ̂�1����n�߂�)
    [Header("�G�L������")]
    public String enemyName;                            //�G�̖��O
    [Header("�푰��")]
    public String enemyRace;                            //�G�̎푰
    [Header("�G�̑���")]
    public int enemyAttribute = 1;                      //�G�̑���
    [Header("�G�̍U����")]
    public int enemyAttack = 100;                       //�G�̍U����(�����ɂ͊�b�U����)
    [Header("�G�̍U���͂̊�b�{��")]
    public float enemyAttackBaseRate = 1;               //�G�̍U���͂̊�b�{��
    [Header("�G�̍ő�HP")]
    public int enemyMaxHp = 10000;                      //�G�̍ő�HP
    [Header("�G�̍ő�HP�̊�b�{��")]
    public float enemyMaxHpBaseRate = 1;                //�G�̍ő�HP�̊�b�{��
    [Header("�G�C���X�g")]
    public Sprite enemyPicture;                         //�G�̃C���X�g
}
